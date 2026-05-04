using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    [Authorize]
    [HttpGet("protected")]
    public IActionResult Protected()
    {
        return Ok("Acceso autorizado OK");
    }

    [Authorize]
    [HttpGet("refresh-check")]
    public async Task<IActionResult> RefreshCheck()
    {
        await Task.Delay(3000); // fuerza uso de token en tiempo
        return Ok("Refresh OK");
    }
}