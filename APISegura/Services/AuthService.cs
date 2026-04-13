using APISegura.Entities;
using APISegura.Repositories;

namespace APISegura.Services;

public class AuthService
{
    private readonly IUserRepository _repo;
    private readonly IRefreshTokenRepository _refreshRepo;
    private readonly IConfiguration _config;
    private readonly JwtService _jwt;
    private readonly PasswordService _pwd;    
    private readonly TokenService _tokenService;
   

    public AuthService(
        IUserRepository repo,
        IRefreshTokenRepository refreshRepo,
        JwtService jwt,
        PasswordService pwd,       
        TokenService tokenService,
        IConfiguration config)
    {
        _repo = repo;
        _jwt = jwt;
        _pwd = pwd;
        _refreshRepo = refreshRepo;
        _tokenService = tokenService;
        _config = config;
    }

    public async Task<(string accessToken, string refreshToken)?> Login(string username, string password)
    {
        var jwt = _config.GetSection("Jwt");
        var expirationDays = int.Parse(jwt["RefreshTokenExpirationDays"]);

        var user = await _repo.GetByUsername(username);
        if (user == null) return null;

        var ok = _pwd.Verify(password, user.PasswordHash, user.PasswordSalt, user.Iterations);
        if (!ok) return null;

        var accessToken = _jwt.GenerateToken(user.Username, user.Role, user.Id);

        var refreshToken = _tokenService.GenerateRefreshToken();

        await _refreshRepo.Save(new RefreshToken
        {
            UserId = user.Id,
            Token = refreshToken,
            Expiration = DateTime.UtcNow.AddDays(expirationDays),
            Created = DateTime.UtcNow
        });

        return (accessToken, refreshToken);
    }

    public async Task<(string accessToken, string refreshToken)?> Refresh(string refreshToken)
    {
        var stored = await _refreshRepo.Get(refreshToken);

        if (stored == null || stored.IsRevoked || stored.Expiration < DateTime.UtcNow)
            return null;

        var user = await _repo.GetById(stored.UserId);
        if (user == null) return null;

        var newAccessToken = _jwt.GenerateToken(user.Username, user.Role, user.Id);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        var days = int.Parse(_config["Jwt:RefreshTokenExpirationDays"]);

        // 🔒 ROTACIÓN
        stored.IsRevoked = true;
        stored.RevokedAt = DateTime.UtcNow;
        stored.ReplacedByToken = newRefreshToken;

        await _refreshRepo.Update(stored);

        // 💾 nuevo refresh token
        await _refreshRepo.Save(new RefreshToken
        {
            UserId = user.Id,
            Token = newRefreshToken,
            Expiration = DateTime.UtcNow.AddDays(days),
            Created = DateTime.UtcNow
        });

        return (newAccessToken, newRefreshToken);
    }

    public async Task<(bool ok, string? error)> Register(string username, string password, string role)
    {
        var exists = await _repo.GetByUsername(username);
        if (exists != null) return (false, "Usuario ya existe");

        var (hash, salt, it) = _pwd.HashPassword(password);

        var user = new User
        {
            Username = username,
            PasswordHash = hash,
            PasswordSalt = salt,
            Iterations = it,
            Role = role
        };

        await _repo.Create(user);
        return (true, null);
    }

    public async Task Logout(string refreshToken)
    {
        var stored = await _refreshRepo.Get(refreshToken);

        if (stored == null) return;

        stored.IsRevoked = true;
        stored.RevokedAt = DateTime.UtcNow;

        await _refreshRepo.Update(stored);
    }
}