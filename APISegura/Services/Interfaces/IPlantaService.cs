using APISegura.Common;
using APISegura.Dtos.Common;
using APISegura.Dtos.Planta;
using APISegura.Entities;

namespace APISegura.Services.Interfaces
{
    public interface IPlantaService
    {
        Task<List<PlantaDto>> Search(string? filtro);
        Task<Result> UpdateCodigoOperacional(int codigo, string nuevoCodigo);
        Task<PagedResult<Planta>> GetPagedAsync(int pageNumber, int pageSize, string? filtro);
    }
}
