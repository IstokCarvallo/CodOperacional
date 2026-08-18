using APISegura.Dtos.Causales;
using APISegura.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
            [FromQuery] string? filtro, CancellationToken ct)
        {
            var result = await _service.GetEspeciesAsync(filtro, ct);

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
            CancellationToken ct = default)
        {
            var result = await _service.GetByEspecieAsync(Codigo, pageNumber, pageSize, filtro, ct);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateCausalRequest request,
            CancellationToken ct)
        {
            var result =
                await _service.CreateAsync(request, ct);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("{causalId:int}")]
        public async Task<IActionResult> Update(
            int causalId,
            [FromBody] UpdateCausalRequest request,
            CancellationToken ct)
        {
            var result = await _service.UpdateAsync(causalId, request, ct);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPatch("{causalId:int}/active")]
        public async Task<IActionResult> SetActive(
            int causalId,
            [FromBody] SetCausalActiveRequest request,
            CancellationToken ct)
        {
            var result =
                await _service.SetActiveAsync(causalId, request.Activo, ct);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
