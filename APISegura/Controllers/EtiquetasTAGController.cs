using APISegura.Dtos.EtiquetasTAG;
using APISegura.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APISegura.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Calidad")]
    public class EtiquetasTAGController : ControllerBase
    {
        private readonly IEtiquetaTAGService _service;

        public EtiquetasTAGController(
            IEtiquetaTAGService service)
        {
            _service = service;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string? filtro,
            CancellationToken cancellationToken)
        {
            var result = await _service.SearchAsync(filtro, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateEtiquetaTAGRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _service.CreateAsync(request, cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
