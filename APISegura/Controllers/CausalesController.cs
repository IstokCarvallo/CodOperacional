using APISegura.Dtos.Causales;
using APISegura.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APISegura.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Calidad")]
    public class CausalesController : ControllerBase
    {
        private readonly ICausalService _service;

        public CausalesController(
            ICausalService service)
        {
            _service = service;
        }

        [HttpGet("especies")]
        public async Task<IActionResult> GetEspecies(
            [FromQuery] string? filtro,
            CancellationToken cancellationToken)
        {
            var result = await _service.GetEspeciesAsync(filtro,
                                            cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }


        [HttpGet("especie/{Codigo:int}")]
        public async Task<IActionResult> GetByEspecie(
            int Codigo,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? filtro = null,
            CancellationToken cancellationToken = default)
        {
            var result = await _service.GetByEspecieAsync(Codigo, pageNumber, pageSize, filtro, cancellationToken);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateCausalRequest request,
            CancellationToken cancellationToken)
        {
            var result =
                await _service.CreateAsync(request, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPatch("{causalId:int}/active")]
        public async Task<IActionResult> SetActive(
            int causalId,
            [FromBody] SetCausalActiveRequest request,
            CancellationToken cancellationToken)
        {
            var result =
                await _service.SetActiveAsync(
                    causalId,
                    request.Activo,
                    cancellationToken);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
