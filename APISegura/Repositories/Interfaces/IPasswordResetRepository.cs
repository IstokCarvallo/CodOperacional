using APISegura.Entities;

namespace APISegura.Repositories.Interfaces
{
    public interface IPasswordResetRepository
    {
        Task Create(PasswordResetToken token);
        Task<PasswordResetToken?> GetByToken(string token);
        Task InvalidateAllByUser(int userId);
        Task Update(PasswordResetToken token);
    }
}
