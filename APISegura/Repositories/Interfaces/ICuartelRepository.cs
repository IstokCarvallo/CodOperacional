using APISegura.Common;
using APISegura.Dtos.Common;
using APISegura.Dtos.Cuarteles;

namespace APISegura.Repositories.Interfaces
{
    public interface ICuartelRepository
    {
        Task<List<CatalogoDto>> GetProductoresAsync(string? filtro);
        Task<List<CatalogoDto>> GetPrediosAsync(int productor, string? filtro);

        Task<List<CuartelDto>> SearchAsync(int productor, int predio, string? filtro);
        Task<Result> UpdateCodigoOperacionalAsync(
            int productor,
            int predio,
            int codigoCuartel,
            string nuevoCodigo,
            string usuario
        );
    }
}
