using APISegura.Dtos.Auth;
using APISegura.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var token = await _authService.Login(request.Username, request.Password);

        if (token == null)
            return Unauthorized();

        return Ok(new { token });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var (ok, error) = await _authService.Register(request.Username, request.Password, request.Role);
        if (!ok) return BadRequest(error);

        return Ok("Usuario creado");
    }
}