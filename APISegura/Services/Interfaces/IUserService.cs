using APISegura.Common;
using APISegura.Dtos.Common;
using APISegura.Dtos.Users;

namespace APISegura.Services.Interfaces
{
    public interface IUserService
    {
        Task<PagedResult<UserListDto>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? filtro);

        Task<UserDetailDto?> GetByIdAsync(int id);

        Task<bool> UpdateAsync(
            int id,
            UpdateUserRequest request,
            string updatedBy);

        Task<Result> SetActiveAsync(
            int id,
            bool active,
            string updatedBy);
    }
}
