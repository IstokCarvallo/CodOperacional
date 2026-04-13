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
        var result = await _authService.Login(request.Username, request.Password);

        if (result == null)
            return Unauthorized();

        return Ok(new AuthResponse
        {
            AccessToken = result.Value.accessToken,
            RefreshToken = result.Value.refreshToken
        });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshRequest request)
    {
        var result = await _authService.Refresh(request.RefreshToken);

        if (result == null)
            return Unauthorized();

        return Ok(new AuthResponse
        {
            AccessToken = result.Value.accessToken,
            RefreshToken = result.Value.refreshToken
        });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var (ok, error) = await _authService.Register(request.Username, request.Password, request.Role);
        if (!ok) return BadRequest(error);

        return Ok("Usuario creado");
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(RefreshRequest request)
    {
        await _authService.Logout(request.RefreshToken);
        return Ok();
    }
}