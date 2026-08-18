using APISegura.Common;
using APISegura.Dtos.Common;
using APISegura.Dtos.Folios;
using APISegura.Repositories.Interfaces;
using APISegura.Services.Interfaces;

namespace APISegura.Services
{
    public class FolioService : IFolioService
    {
        private readonly IFolioRepository _repository;

        public FolioService(IFolioRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<PagedResult<FolioBusquedaDto>>> SearchAsync(
            int pageNumber,
            int pageSize,
            string? filtro,
            CancellationToken cancellationToken)
        {
            return await _repository.SearchAsync(
                pageNumber,
                pageSize,
                filtro,
                cancellationToken);
        }
    }
}
