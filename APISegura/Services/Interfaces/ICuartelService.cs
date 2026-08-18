using APISegura.Common;
using APISegura.Dtos.Common;
using APISegura.Dtos.Cuarteles;

namespace APISegura.Services.Interfaces
{
    public interface ICuartelService
    {
        Task<List<CatalogoDto>> GetProductores(string? filtro);
        Task<List<CatalogoDto>> GetPredios(int productor, string? filtro);
        Task<List<CuartelDto>> Search(int productor, int predio, string? filtro);
        Task<Result> Update(UpdateCodigoOperacionalCuartelDto dto);
    }
}
