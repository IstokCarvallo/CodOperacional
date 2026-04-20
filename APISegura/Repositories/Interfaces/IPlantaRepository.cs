using APISegura.Common;
using APISegura.Dtos.Planta;

namespace APISegura.Repositories.Interfaces
{
    public interface IPlantaRepository
    {
        Task<List<PlantaDto>> SearchAsync(string? filtro);
        Task<Result> UpdateCodigoOperacionalAsync(int codigo, string nuevoCodigo, string usuario);
    }
}
