using APISegura.Entities;

namespace APISegura.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByUsername(string username);
        Task<User?> GetById(int id);
        Task<int> Create(User user);
        Task Update(User user);
        Task UpdatePassword(User user);
        Task UpdateSecurityStamp(User user);
    }
}
