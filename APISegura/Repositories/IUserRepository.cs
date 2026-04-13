using APISegura.Entities;

namespace APISegura.Repositories;
public interface IUserRepository
{
    Task<User?> GetByUsername(string username);
    Task<int> Create(User user);
}