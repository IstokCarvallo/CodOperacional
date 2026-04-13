using APISegura.Repositories;
using APISegura.Entities;

namespace APISegura.Services;

public class AuthService
{
    private readonly IUserRepository _repo;
    private readonly JwtService _jwt;
    private readonly PasswordService _pwd;
    private readonly IRefreshTokenRepository _refreshRepo;
    private readonly TokenService _tokenService;

    public AuthService(
        IUserRepository repo,
        JwtService jwt,
        PasswordService pwd,
        IRefreshTokenRepository refreshRepo,
        TokenService tokenService)
    {
        _repo = repo;
        _jwt = jwt;
        _pwd = pwd;
        _refreshRepo = refreshRepo;
        _tokenService = tokenService;
    }

    public async Task<(string accessToken, string refreshToken)?> Login(string username, string password)
    {
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
            Expiration = DateTime.UtcNow.AddDays(7)
        });

        return (accessToken, refreshToken);
    }

    public async Task<(string accessToken, string refreshToken)?> Refresh(string refreshToken)
    {
        var stored = await _refreshRepo.Get(refreshToken);

        if (stored == null || stored.IsRevoked || stored.Expiration < DateTime.UtcNow)
            return null;

        // 🔒 revocamos el token usado
        await _refreshRepo.Revoke(refreshToken);

        var user = await _repo.GetById(stored.UserId);
        if (user == null) return null;

        // 🎫 nuevo access token
        var newAccessToken = _jwt.GenerateToken(user.Username, user.Role, user.Id);

        // 🔁 nuevo refresh token (ROTACIÓN)
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        await _refreshRepo.Save(new RefreshToken
        {
            UserId = user.Id,
            Token = newRefreshToken,
            Expiration = DateTime.UtcNow.AddDays(7)
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

        await _refreshRepo.Revoke(refreshToken);
    }
}