using APISegura.Dtos.Cuarteles;
using APISegura.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APISegura.Controllers
{
    [ApiController]
    [Route("api/cuarteles")]
    [Authorize]
    public class CuartelesController : Controller
    {
        private readonly ICuartelService _service;

        public CuartelesController(ICuartelService service)
        {
            _service = service;
        }

        [HttpGet("productores")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> GetProductores(string? filtro)
        {
            var data = await _service.GetProductores(filtro);
            return Json(data);
        }

        [HttpGet("predios")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult>GetPredios(int productor, string? filtro)
        {
            var data = await _service.GetPredios(productor, filtro);
            return Json(data);
        }

        [HttpGet]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult>Get(int productor, int predio, string? filtro)
        {
            var data = await _service.Search(productor, predio, filtro);
            return Json(data);
        }

        [HttpPost("codigo-operacional")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult>Update(UpdateCodigoOperacionalCuartelDto dto)
        {
            var result = await _service.Update(dto);

            if (!result.IsSuccess)
                return Json(new { success = false, message = result.Message });

            return Json(new { success = true });
        }
    }
}
