using APISegura.Dtos.Auth;
using APISegura.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

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
    [EnableRateLimiting("login-ip-policy")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var result = await _authService.Login(request.Username, request.Password);

        if (!result.Success)
            return Unauthorized(new { message = result.Error });

        return Ok(new AuthResponse
        {
            AccessToken = result.Data.AccessToken,
            RefreshToken = result.Data.RefreshToken
        });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshRequest request)
    {
        var result = await _authService.Refresh(request.RefreshToken);

        if (!result.Success)
            return Unauthorized(new { message = result.Error });

        return Ok(new AuthResponse
        {
            AccessToken = result.Data.AccessToken,
            RefreshToken = result.Data.RefreshToken
        });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var result = await _authService.Register(request.Username, request.Password, request.Role);
        if (!result.Success)
            return BadRequest(new { message = result.Error });

        return Ok(new { message = "Usuario creado" });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(RefreshRequest request)
    {
        var result = await _authService.Logout(request.RefreshToken);
        if (!result.Success)
            return BadRequest(new { message = result.Error });

        return Ok(new { message = "Logout exitoso" });
    }
}