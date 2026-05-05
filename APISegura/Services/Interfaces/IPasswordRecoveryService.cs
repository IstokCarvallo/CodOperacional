using APISegura.Common;

namespace APISegura.Services.Interfaces
{
    public interface IPasswordRecoveryService
    {
        Task<Result<bool>> ForgotPassword(string email);
        Task<Result<bool>> ResetPassword(string token, string newPassword);
    }
}
