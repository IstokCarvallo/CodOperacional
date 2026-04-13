using APISegura.Repositories;
using APISegura.Entities;

namespace APISegura.Services;

public class AuthService
{
    private readonly IUserRepository _repo;
    private readonly JwtService _jwt;
    private readonly PasswordService _pwd;

    public AuthService(IUserRepository repo, JwtService jwt, PasswordService pwd)
    {
        _repo = repo;
        _jwt = jwt;
        _pwd = pwd;
    }

    public async Task<string?> Login(string username, string password)
    {
        var user = await _repo.GetByUsername(username);
        if (user == null) return null;

        var ok = _pwd.Verify(password, user.PasswordHash, user.PasswordSalt, user.Iterations);
        if (!ok) return null;

        return _jwt.GenerateToken(user.Username, user.Role, user.Id);
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
}