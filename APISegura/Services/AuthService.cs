using APISegura.Common;
using APISegura.Common.Validators;
using APISegura.Dtos.Auth;
using APISegura.Entities;
using APISegura.Repositories.Interfaces;

namespace APISegura.Services;

public class AuthService
{
    private readonly IUserRepository _repo;
    private readonly IRefreshTokenRepository _refreshRepo;
    private readonly IConfiguration _config;
    private readonly ILogger<AuthService> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly JwtService _jwt;
    private readonly PasswordService _pwd;    
    private readonly TokenService _tokenService;

    public AuthService(
        IUserRepository repo,
        IRefreshTokenRepository refreshRepo,
        JwtService jwt,
        PasswordService pwd,       
        TokenService tokenService,
        IConfiguration config,
        ILogger<AuthService> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        _repo = repo;
        _jwt = jwt;
        _pwd = pwd;
        _refreshRepo = refreshRepo;
        _tokenService = tokenService;
        _config = config;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<AuthResponse>> Login(string username, string password)
    {
        _logger.LogInformation("Intento de inicio de sesión para {Username}, a las {time}", username, DateTime.UtcNow);

        try
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                _logger.LogWarning("Error al iniciar sesión: nombre de usuario vacío en {time}", DateTime.UtcNow);
                return Result<AuthResponse>.Fail("Username requerido");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                _logger.LogWarning("Error de inicio de sesión: contraseña vacía para {Username} en {time}", username, DateTime.UtcNow);
                return Result<AuthResponse>.Fail("Password requerido");
            }

            var jwt = _config.GetSection("Jwt");
            var expirationDays = int.Parse(jwt["RefreshTokenExpirationDays"]);

            var user = await _repo.GetByUsername(username);
            if (user == null)
            {
                _logger.LogWarning("Error de inicio de sesión: usuario no encontrado: {Username} en {time}", username, DateTime.UtcNow);
                return Result<AuthResponse>.Fail("Credenciales inválidas");
            }

            // 🔒 Verificar si está bloqueado
            if (user.LockoutUntil.HasValue && user.LockoutUntil > DateTime.UtcNow)
            {
                _logger.LogWarning("Usuario bloqueado {UserId} hasta {LockoutUntil}", user.Id, user.LockoutUntil);
                return Result<AuthResponse>.Fail("Credenciales inválidas");
            }

            var ok = _pwd.Verify(password, user.PasswordHash, user.PasswordSalt, user.Iterations);
            if (!ok)
            {
                user.FailedAttempts++;

                _logger.LogWarning("Password incorrecta para {UserId}. Intento #{Attempts}", user.Id, user.FailedAttempts);

                if (user.FailedAttempts >= 5)
                {
                    user.LockoutUntil = DateTime.UtcNow.AddMinutes(5);
                    _logger.LogWarning("Usuario {UserId} bloqueado hasta {LockoutUntil} debido a múltiples intentos fallidos", user.Id, user.LockoutUntil);
                }

                await _repo.Update(user);

                return Result<AuthResponse>.Fail("Credenciales inválidas");
            }

            user.FailedAttempts = 0;
            user.LockoutUntil = null;
            await _repo.Update(user);

            var accessToken = _jwt.GenerateToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();
            var ip = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
            var userAgent = _httpContextAccessor.HttpContext?.Request.Headers["User-Agent"].ToString();

            await _refreshRepo.Save(new RefreshToken
            {
                UserId = user.Id,
                Token = refreshToken,
                Expiration = DateTime.UtcNow.AddDays(expirationDays),
                Created = DateTime.UtcNow,
                IpAddress = ip,
                UserAgent = userAgent,
                Device = ParseDevice(userAgent)
            });

            _logger.LogInformation("Inicio de sesión exitoso para el ID de usuario {UserId} en {time}", user.Id, DateTime.UtcNow);

            return Result<AuthResponse>.Ok(new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error no controlado durante el inicio de sesión para {Username} en {time}", username, DateTime.UtcNow);
            throw;
        }
    }

    public async Task<Result<AuthResponse>> Refresh(string refreshToken)
    {
        _logger.LogInformation("Intento de actualización del token en {time}", DateTime.UtcNow);

        try
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                _logger.LogWarning("Error al actualizar: token vacío en {time}", DateTime.UtcNow);
                return Result<AuthResponse>.Fail("Token inválido");
            }

            var stored = await _refreshRepo.Get(refreshToken);

            if (stored == null)
            {
                _logger.LogWarning("Error al actualizar: token no encontrado en {time}", DateTime.UtcNow);
                return Result<AuthResponse>.Fail("Token inválido");
            }

            if (stored.IsRevoked)
            {
                _logger.LogWarning("Posible reuse attack para UserId {UserId}", stored.UserId);
                await _refreshRepo.RevokeAllByUser(stored.UserId);
                var compromisedUser = await _repo.GetById(stored.UserId);
                if (compromisedUser != null)
                {
                    compromisedUser.SecurityStamp = Guid.NewGuid().ToString();
                    await _repo.UpdateSecurityStamp(compromisedUser);
                }
                return Result<AuthResponse>.Fail("Token comprometido");
            }

            if (stored.Expiration < DateTime.UtcNow)
            {
                _logger.LogWarning("Error al actualizar: el token ha caducado para el ID de usuario {UserId} en {time}", stored.UserId, DateTime.UtcNow);
                return Result<AuthResponse>.Fail("Token expirado");
            }

            var user = await _repo.GetById(stored.UserId);
            if (user == null)
            {
                _logger.LogError("Error al actualizar: no se encontró el usuario para el ID de usuario {UserId} en {time}", stored.UserId, DateTime.UtcNow);
                return Result<AuthResponse>.Fail("Credenciales inválidas");
            }

            var newAccessToken = _jwt.GenerateToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            var days = int.Parse(_config["Jwt:RefreshTokenExpirationDays"]);

            // 🔒 ROTACIÓN
            stored.IsRevoked = true;
            stored.RevokedAt = DateTime.UtcNow;
            stored.ReplacedByToken = newRefreshToken;
            stored.LastUsedAt = DateTime.UtcNow;

            await _refreshRepo.Update(stored);

            _logger.LogInformation("Se ha revocado el token de actualización anterior para el ID de usuario {UserId} en {time}", user.Id, DateTime.UtcNow);

            // 💾 nuevo refresh token
            await _refreshRepo.Save(new RefreshToken
            {
                UserId = user.Id,
                Token = newRefreshToken,
                Expiration = DateTime.UtcNow.AddDays(days),
                Created = DateTime.UtcNow,
                IpAddress = stored.IpAddress,
                UserAgent = stored.UserAgent,
                Device = stored.Device,
                LastUsedAt = DateTime.UtcNow
            });

            _logger.LogInformation("Actualización exitosa para el ID de usuario {UserId} en {time}", user.Id, DateTime.UtcNow);

            return Result<AuthResponse>.Ok(new AuthResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error no controlado durante el proceso de actualización del token en {time}", DateTime.UtcNow);
            throw;
        }
    }

    public async Task<Result<bool>> Register(RegisterRequest request)
    {
        _logger.LogInformation("Intento de registro para {Username} en {time}", request.Username, DateTime.UtcNow);

        try
        {
            if (string.IsNullOrWhiteSpace(request.Email))
                return Result<bool>.Fail("Email requerido");

            request.Email = EmailValidator.Normalize(request.Email);

            var existingByEmail = await _repo.GetByEmail(request.Email);
            if (existingByEmail != null)
                return Result<bool>.Fail("Email ya registrado");

            if (string.IsNullOrWhiteSpace(request.Username))
            {
                _logger.LogWarning("Error al registrarse: nombre de usuario vacío en {time}", DateTime.UtcNow);
                return Result<bool>.Fail("Username requerido");
            }

            if (string.IsNullOrWhiteSpace(request.Password))
            {
                _logger.LogWarning("El registro falló: contraseña vacía para {Username} en {time}", request.Username, DateTime.UtcNow);
                return Result<bool>.Fail("Password requerido");
            }

            var exists = await _repo.GetByUsername(request.Username);
            if (exists != null)
            {
                _logger.LogWarning("Error al registrar el registro: el usuario ya existe: {Username} en {time}", request.Username, DateTime.UtcNow);
                return Result<bool>.Fail("Usuario ya existe");
            }

            var (hash, salt, it) = _pwd.HashPassword(request.Password);
            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                Nombre = request.Nombre,
                PasswordHash = hash,
                PasswordSalt = salt,
                Iterations = it,
                SecurityStamp = Guid.NewGuid().ToString(),
                Role = request.Role
            };

            await _repo.Create(user);

            _logger.LogInformation("Registro exitoso para {Username} con el rol {Role} en {time}", request.Username, request.Role, DateTime.UtcNow);

            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error no controlado durante el registro para {Username} en {time}", request.Username, DateTime.UtcNow);
            throw;
        }
    }

    public async Task<Result<bool>> ChangePassword(ChangePasswordRequest req)
    {
        try
        {
            var user = await _repo.GetById(req.UserId);

            if (user == null)
                return Result<bool>.Fail("Usuario no existe");

            var valid = _pwd.Verify(req.CurrentPassword, user.PasswordHash, user.PasswordSalt, user.Iterations);

            if (!valid)
                return Result<bool>.Fail("Password actual incorrecta");

            var (hash, salt, it) = _pwd.HashPassword(req.NewPassword);

            user.PasswordHash = hash;
            user.PasswordSalt = salt;
            user.Iterations = it;
            user.SecurityStamp = Guid.NewGuid().ToString();

            await _repo.UpdatePassword(user);

            await _refreshRepo.RevokeAllByUser(user.Id);

            _logger.LogInformation("Password cambiada para UserId {UserId}", user.Id);

            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cambiando password");
            throw;
        }
    }

    public async Task<List<SessionDto>> GetSessions(int userId)
    {
        var tokens = await _refreshRepo.GetActiveByUser(userId);

        return tokens.Select(t => new SessionDto
        {
            Token = t.Token,
            Created = t.Created,
            LastUsedAt = t.LastUsedAt,
            IpAddress = t.IpAddress,
            Device = t.Device
        }).ToList();
    }

    public async Task<Result<bool>> RevokeSession(string token)
    {
        var stored = await _refreshRepo.Get(token);

        if (stored == null)
            return Result<bool>.Fail("Sesión no existe");

        stored.IsRevoked = true;
        stored.RevokedAt = DateTime.UtcNow;
        stored.RevokedReason = "User logout";

        await _refreshRepo.Update(stored);

        return Result<bool>.Ok(true);
    }

    public async Task<Result<bool>> Logout(string refreshToken)
    {
        _logger.LogInformation("Intento de cierre de sesión en {time}", DateTime.UtcNow);

        try
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                _logger.LogWarning("Error al cerrar sesión: token de actualización vacío en {time}", DateTime.UtcNow);
                return Result<bool>.Fail("Token inválido");
            }
            var stored = await _refreshRepo.Get(refreshToken);

            if (stored == null)
            {
                _logger.LogWarning("Error al cerrar sesión: no se encontró el token de actualización en {time}", DateTime.UtcNow);
                return Result<bool>.Fail("Token inválido");
            }

            if (stored.IsRevoked)
            {
                _logger.LogInformation("Cierre de sesión omitido: el token ya ha sido revocado para el UserId {UserId} en {time}", stored.UserId, DateTime.UtcNow);
                return Result<bool>.Ok(true);
            }

            stored.IsRevoked = true;
            stored.RevokedAt = DateTime.UtcNow;

            await _refreshRepo.Update(stored);

            _logger.LogInformation("Cierre de sesión exitoso para UserId {UserId} en {time}", stored.UserId, DateTime.UtcNow);

            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error no controlado durante el cierre de sesión  en {time}", DateTime.UtcNow);
            throw;
        }
    }

    public async Task<Result<bool>> LogoutOthers(int userId, string currentToken)
    {
        await _refreshRepo.RevokeAllExcept(userId, currentToken);

        return Result<bool>.Ok(true);
    }

    public async Task<Result<bool>> LogoutAll(int userId)
    {
        try
        {
            var user = await _repo.GetById(userId);

            if (user == null)
                return Result<bool>.Fail("Usuario no existe");

            // Revocar TODOS los refresh tokens
            await _refreshRepo.RevokeAllByUser(userId);

            // Invalidar TODOS los JWT (via SecurityStamp)
            user.SecurityStamp = Guid.NewGuid().ToString();

            await _repo.UpdateSecurityStamp(user);

            _logger.LogInformation("Todas las sesiones cerradas para UserId {UserId}", userId);

            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cerrando sesiones para UserId {UserId}", userId);
            throw;
        }
    }
    private string ParseDevice(string userAgent)
    {
        if (string.IsNullOrEmpty(userAgent))
            return "Unknown";

        if (userAgent.Contains("Chrome"))
            return "Chrome";

        if (userAgent.Contains("Firefox"))
            return "Firefox";

        if (userAgent.Contains("Edg"))
            return "Edge";

        return "Other";
    }
}