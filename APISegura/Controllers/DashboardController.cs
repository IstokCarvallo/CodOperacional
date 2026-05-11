using APISegura.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APISegura.Controllers;

[ApiController]
[Route("api/dashboard")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _service;
    private readonly ILogger<DashboardController> _logger;

    public DashboardController(
        IDashboardService service,
        ILogger<DashboardController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet("resumen")]
    public async Task<IActionResult> ObtenerResumen()
    {
        try
        {
            var result =
                await _service.ObtenerResumenAsync();

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener resumen dashboard");

            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    mensaje = "Error interno al obtener dashboard."
                });
        }
    }

    [HttpGet("ultimos-cuarteles")]
    public async Task<IActionResult> ObtenerUltimosCuarteles()
    {
        try
        {
            var result = await _service.ObtenerUltimosCuartelesAsync();

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener últimos cuarteles");

            return StatusCode(StatusCodes.Status500InternalServerError,
                new
                {
                    mensaje = "Error interno al obtener cuarteles."
                });
        }
    }

    [HttpGet("ultimas-plantas")]
    public async Task<IActionResult> ObtenerUltimasPlantas()
    {
        try
        {
            var result = await _service.ObtenerUltimasPlantasAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener últimas plantas");

            return StatusCode(StatusCodes.Status500InternalServerError,
                new
                {
                    mensaje = "Error interno al obtener plantas."
                });
        }
    }

    [HttpGet("cajas-por-codigo")]
    public async Task<IActionResult> ObtenerCajasPorCodigo()
    {
        try
        {
            var result = await _service.ObtenerCajasPorCodigoAsync();

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener cajas por código");

            return StatusCode(StatusCodes.Status500InternalServerError,
                new
                {
                    mensaje = "Error interno al obtener cajas."
                });
        }
    }

    [HttpGet("total-cajas-hoy")]
    public async Task<IActionResult> ObtenerTotalCajasHoy()
    {
        try
        {
            var result = await _service.ObtenerTotalCajasHoyAsync();

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener total cajas hoy");

            return StatusCode(StatusCodes.Status500InternalServerError,
                new
                {
                    mensaje = "Error interno al obtener cajas hoy."
                });
        }
    }

    [HttpGet("total-pallets-hoy")]
    public async Task<IActionResult> ObtenerTotalPalletsHoy()
    {
        try
        {
            var result = await _service.ObtenerTotalPalletsHoyAsync();

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener total pallets hoy");

            return StatusCode(StatusCodes.Status500InternalServerError,
                new
                {
                    mensaje = "Error interno al obtener pallets hoy."
                });
        }
    }
}