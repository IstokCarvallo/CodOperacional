using APISegura.Dtos.Causales;
using APISegura.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APISegura.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CausalesController : ControllerBase
    {
        private readonly ICausalService _service;

        public CausalesController(
            ICausalService service)
        {
            _service = service;
        }

        [HttpGet("especie/{espeCodigo:int}")]
        public async Task<IActionResult> GetByEspecie(
            int espeCodigo,
            CancellationToken cancellationToken)
        {
            var result =
                await _service.GetByEspecieAsync(espeCodigo, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

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
