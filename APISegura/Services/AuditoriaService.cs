using APISegura.Common;
using APISegura.Dtos.Common;
using APISegura.Repositories.Interfaces;

namespace APISegura.Services
{
    public class AuditoriaService
    {
        private readonly IAuditoriaRepository _repo;

        public AuditoriaService(IAuditoriaRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<AuditoriaDto>> ObtenerAsync(
            DateTime? desde,
            DateTime? hasta,
            string? usuario,
            string? entidad,
            string? accion)
        {
            // puedes agregar validaciones aquí
            if (desde.HasValue && hasta.HasValue && desde > hasta)
                throw new ArgumentException("Rango de fechas inválido");

            return await _repo.GetAsync(desde, hasta, usuario, entidad, accion);
        }

        public async Task<AuditoriaDto?> ObtenerPorIdAsync(long id)
        {
            return await _repo.GetByIdAsync(id);
        }
    }
}
