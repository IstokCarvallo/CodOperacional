using DesktopCodOperacional.Models.Common;
using DesktopCodOperacional.Models.Cuartel;

namespace DesktopCodOperacional.Services
{
    public class CuartelService
    {
        private readonly ApiService _apiService;

        public CuartelService(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<List<ProductorDto>> ObtenerProductoresAsync()
        {
            var result = await _apiService.GetAsync<List<ProductorDto>>("api/cuarteles/productores");

            return result ?? new List<ProductorDto>();
        }

        public async Task<List<PredioDto>> ObtenerPrediosAsync(int productor)
        {
            var result = await _apiService.GetAsync<List<PredioDto>>($"api/cuarteles/predios?productor={productor}");

            return result ?? new List<PredioDto>();
        }

        public async Task<List<CuartelDto>> ObtenerCuartelesAsync(int productor, int predio)
        {
            var result = await _apiService.GetAsync<List<CuartelDto>>($"api/cuarteles?productor={productor}&predio={predio}");

            return result ?? new List<CuartelDto>();
        }

        public async Task<ApiResponseDto> ActualizarCodigoAsync(UpdateCodigoOperacionalDto dto)
        {
            var result = await _apiService.PostAsync<ApiResponseDto>("api/cuarteles/codigo-operacional", dto);

            return result ?? new ApiResponseDto
            {
                Success = false,
                Message = "Error de comunicación con el servidor"
            };
        }
    }
}
