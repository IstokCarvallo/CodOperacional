using FrontCodOperacional.Models.Causal;
using FrontCodOperacional.Models.Planta;

namespace FrontCodOperacional.Services.Api.Interfaces
{
    public interface ICausalesService
    {
        Task<List<CausalEspecieDto>> GetEspecies(string? filtro = null,
            CancellationToken cancellationToken = default);

        Task<PagedResult<CausalDto>?> GetByEspecie(
            int especieCodigo,
            int pageNumber,
            int pageSize,
            string? filtro = null,
            CancellationToken cancellationToken = default);

        Task Create(
            CreateCausalRequest request,
            CancellationToken cancellationToken = default);

        Task Update(
            int causalId,
            UpdateCausalRequest request,
            CancellationToken cancellationToken = default);

        Task SetActive(
            int causalId,
            bool activo,
            CancellationToken cancellationToken = default);
    }
}
