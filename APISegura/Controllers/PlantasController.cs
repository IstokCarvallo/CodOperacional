using APISegura.Dtos.Planta;
using APISegura.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APISegura.Controllers
{
    [ApiController]
    [Route("api/plantas")]
    [Authorize]
    public class PlantasController : Controller
    {
        private readonly PlantaService _service;

        public PlantasController(PlantaService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Get(string? filtro)
        {
            var result = await _service.Search(filtro);
            return Json(result);
        }

        [HttpPost("codigo-operacional")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Update(UpdateCodigoOperacionalDto dto)
        {
            var result = await _service.UpdateCodigoOperacional(
                dto.Codigo,
                dto.CodigoOperacional
            );

            if (!result.IsSuccess)
                return Json(new { success = false, message = result.Message });

            return Json(new { success = true });
        }

        [HttpGet("paged")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _service.GetPagedAsync(pageNumber, pageSize);
            return Ok(result);
        }
    }
}
