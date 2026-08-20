using APISegura.Dtos.Inspecciones;
using APISegura.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace APISegura.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class InspeccionesController : ControllerBase
    {
        private readonly IInspeccionService _service;
        private readonly IFolioService _folioService;

        public InspeccionesController(
            IInspeccionService service, 
            IFolioService folioService)
        {
            _service = service;
            _folioService = folioService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateInspeccionRequest request,
            CancellationToken cancellationToken)
        {
            var usuarioIdClaim =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(usuarioIdClaim, out var usuarioId))
            {
                return Unauthorized(new
                {
                    Success = false,
                    Error = "No fue posible identificar al usuario autenticado."
                });
            }

            var result = await _service.CreateAsync(request, usuarioId, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("{inspeccionId:long}")]
        public async Task<IActionResult> Update(
            long inspeccionId,
            [FromBody] CreateInspeccionRequest request,
            CancellationToken cancellationToken)
        {
            var usuarioIdClaim =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(usuarioIdClaim, out var usuarioId))
            {
                return Unauthorized(new
                {
                    Success = false,
                    Error = "No fue posible identificar al usuario autenticado."
                });
            }

            var result = await _service.UpdateAsync(
                inspeccionId,
                request,
                usuarioId,
                cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("{inspeccionId:long}")]
        public async Task<IActionResult> GetById(
            long inspeccionId,
            CancellationToken cancellationToken)
        {
            var result = await _service.GetByIdAsync(inspeccionId, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            if (result.Data is null)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("folios")]
        public async Task<IActionResult> BuscarFolios(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? filtro = null,
        CancellationToken cancellationToken = default)
        {
            var result = await _folioService.SearchAsync(
                pageNumber,
                pageSize,
                filtro,
                cancellationToken);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetPaged(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? filtro = null,
            CancellationToken cancellationToken = default)
        {
            var result = await _service.GetPagedAsync(pageNumber, pageSize, filtro, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
