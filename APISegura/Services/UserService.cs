using APISegura.Common;
using APISegura.Dtos.Common;
using APISegura.Dtos.Users;
using APISegura.Repositories.Interfaces;
using APISegura.Services.Interfaces;

namespace APISegura.Services
{
    public sealed class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<PagedResult<UserListDto>>GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? filtro)
        {
            return await _userRepository.GetPagedAsync(
                pageNumber,
                pageSize,
                filtro);
        }

        public async Task<UserDetailDto?> GetByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<bool>UpdateAsync(int id, UpdateUserRequest request, string updatedBy)
        {
            return await _userRepository.UpdateAsync(id, request, updatedBy);
        }

        public async Task<Result> SetActiveAsync(int id, bool active, string updatedBy)
        {
            return await _userRepository.SetActiveAsync(id, active, updatedBy);
        }
    }
}
