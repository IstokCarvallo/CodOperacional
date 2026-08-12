using APISegura.Common;
using APISegura.Dtos.Common;
using APISegura.Dtos.Inspecciones;
using APISegura.Repositories.Interfaces;
using APISegura.Services.Interfaces;

namespace APISegura.Services
{
    public class InspeccionService : IInspeccionService
    {
        private readonly IInspeccionRepository _repository;

        public InspeccionService(
            IInspeccionRepository repository)
        {
            _repository = repository;
        }

        public Task<Result<long>> CreateAsync(
            CreateInspeccionRequest request,
            int usuarioId,
            CancellationToken cancellationToken)
        {
            return _repository.CreateAsync(
                request,
                usuarioId,
                cancellationToken);
        }

        public Task<Result<InspeccionDto?>> GetByIdAsync(
            long inspeccionId,
            CancellationToken cancellationToken)
        {
            return _repository.GetByIdAsync(
                inspeccionId,
                cancellationToken);
        }

        public Task<Result<PagedResult<InspeccionListDto>>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? filtro,
            CancellationToken cancellationToken)
        {
            return _repository.GetPagedAsync(
                pageNumber,
                pageSize,
                filtro,
                cancellationToken);
        }
    }
}
