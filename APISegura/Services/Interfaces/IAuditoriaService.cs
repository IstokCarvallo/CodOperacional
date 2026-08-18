using APISegura.Dtos.Common;

namespace APISegura.Services.Interfaces
{
    public interface IAuditoriaService
    {

        Task<IEnumerable<AuditoriaDto>> ObtenerAsync(
            DateTime? desde, DateTime? hasta,
            string? usuario, string? entidad, string? accion);


        Task<AuditoriaDto?> ObtenerPorIdAsync(long id);
    }
}
