using APISegura.Common;
using APISegura.Common.Validators;
using APISegura.Dtos.Auth;
using APISegura.Entities;

namespace APISegura.Services.Interfaces
{
    public interface IAuthService
    {
        Task<Result<AuthResponse>> Login(string username, string password);
        Task<Result<AuthResponse>> Refresh(string refreshToken);
        Task<Result<bool>> Register(RegisterRequest request);
        Task<Result<bool>> ChangePassword(ChangePasswordRequest req);
        Task<List<SessionDto>> GetSessions(int userId);
        Task<Result<bool>> RevokeSession(string token);
        Task<Result<bool>> Logout(string refreshToken);
        Task<Result<bool>> LogoutOthers(int userId, string currentToken);
        Task<Result<bool>> LogoutAll(int userId);
    }
}
