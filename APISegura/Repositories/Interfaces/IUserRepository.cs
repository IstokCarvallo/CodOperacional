using APISegura.Common;
using APISegura.Dtos.Common;
using APISegura.Dtos.Users;
using APISegura.Entities;

namespace APISegura.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByUsername(string username);
        Task<User?> GetByEmail(string email);
        Task<User?> GetById(int id);
        Task<int> Create(User user);
        Task Update(User user);
        Task UpdatePassword(User user);
        Task UpdateSecurityStamp(User user);
        Task<PagedResult<UserListDto>> GetPagedAsync(int pageNumber, int pageSize, string? filtro);
        Task<UserDetailDto?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(int id, UpdateUserRequest request, string updatedBy);
        Task<Result> SetActiveAsync(int id, bool active, string updatedBy);
    }
}
