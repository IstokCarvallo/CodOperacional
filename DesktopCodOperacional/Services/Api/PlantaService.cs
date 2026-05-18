using DesktopCodOperacional.Models.Common;
using DesktopCodOperacional.Models.Planta;

namespace DesktopCodOperacional.Services.Api
{
    public class PlantaService
    {
        private readonly ApiService _apiService;

        public PlantaService(ApiService apiService)
        {
            _apiService = apiService;
        }
        // PAGINADO
        public async Task<PagedResponse<PlantaDto>?>
            ObtenerPaginadoAsync(
                int pageNumber,
                int pageSize)
        {
            return await _apiService.GetAsync<PagedResponse<PlantaDto>>($"api/plantas/paged?pageNumber={pageNumber}&pageSize={pageSize}");
        }

        // FILTRO
        public async Task<List<PlantaDto>>
            BuscarAsync(string filtro)
        {
            var result = await _apiService.GetAsync<List<PlantaDto>>($"api/plantas?filtro={Uri.EscapeDataString(filtro)}");

            return result ?? new List<PlantaDto>();
        }

        public async Task<ApiResponseDto> ActualizarCodigoAsync(UpdateCodigoOperacionalPlantaDto dto)
        {
            var result = await _apiService.PostAsync<ApiResponseDto>("api/plantas/codigo-operacional", dto);

            return result ?? new ApiResponseDto
            {
                Success = false,
                Message = "Error de comunicación con el servidor"
            };
        }
    }
}
