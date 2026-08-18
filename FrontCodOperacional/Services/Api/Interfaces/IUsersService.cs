using FrontCodOperacional.Models.Planta;
using FrontCodOperacional.Models.Users;

namespace FrontCodOperacional.Services.Api.Interfaces
{
    public interface IUsersService
    {
        Task<PagedResult<UserDto>?> GetPaged(int page, int size, string? filter, CancellationToken ct);

        Task<UserDetailDto?> GetById(int id, CancellationToken ct);

        Task Register(RegisterRequest request, CancellationToken ct);

        Task Update(int id, UpdateUserRequest request, CancellationToken ct);

        Task SetStatus(int id, SetUserStatusRequest request, CancellationToken ct);

        Task Create(RegisterRequest request, CancellationToken ct);
    }
}