using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Acceso autorizado");
    }
}