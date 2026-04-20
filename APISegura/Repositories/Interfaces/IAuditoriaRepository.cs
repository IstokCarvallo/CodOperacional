using APISegura.Dtos.Common;

namespace APISegura.Repositories.Interfaces
{
    public interface IAuditoriaRepository
    {
        Task<IEnumerable<AuditoriaDto>> GetAsync(
            DateTime? desde,
            DateTime? hasta,
            string? usuario,
            string? entidad,
            string? accion);

        Task<AuditoriaDto?> GetByIdAsync(long id);
    }
}
