using APISegura.Entities;

namespace APISegura.Repositories.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task Save(RefreshToken token);
        Task<RefreshToken?> Get(string token);
        Task<List<RefreshToken>> GetActiveByUser(int userId);
        Task Revoke(string token);
        Task Update(RefreshToken token);
        Task RevokeAllByUser(int userId);
        Task RevokeAllExcept(int userId, string currentToken);
       
    }
}
