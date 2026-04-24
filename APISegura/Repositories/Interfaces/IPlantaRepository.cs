using APISegura.Common;
using APISegura.Dtos.Planta;
using APISegura.Entities;

namespace APISegura.Repositories.Interfaces
{
    public interface IPlantaRepository
    {
        Task<List<PlantaDto>> SearchAsync(string? filtro);
        Task<Result> UpdateCodigoOperacionalAsync(int codigo, string nuevoCodigo, string usuario);
        Task<(IEnumerable<Planta>, int)> GetPagedAsync(int pageNumber, int pageSize);
    }
}
