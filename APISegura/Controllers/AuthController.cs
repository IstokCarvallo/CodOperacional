using APISegura.Dtos.Auth;
using APISegura.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var result = await _authService.Register(request.Username, request.Password, request.Nombre, request.Role);
        if (!result.Success)
            return BadRequest(new { message = result.Error });

        return Ok(new { message = "Usuario creado" });
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

    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest req)
    {
        var result = await _authService.ChangePassword(req);

        if (!result.Success)
            return BadRequest(result.Error);

        return Ok(new { message = "Cambio de clave exitioso" });
    }

    [Authorize]
    [HttpGet("sessions")]
    public async Task<IActionResult> GetSessions()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        var sessions = await _authService.GetSessions(userId);

        return Ok(sessions);
    }

    [Authorize]
    [HttpPost("logout-session")]
    public async Task<IActionResult> LogoutSession([FromBody] string token)
    {
        var result = await _authService.RevokeSession(token);

        if (!result.Success)
            return BadRequest(new { message = result.Error });

        return Ok(new { message = "Logout exitoso" });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(RefreshRequest request)
    {
        var result = await _authService.Logout(request.RefreshToken);
        if (!result.Success)
            return BadRequest(new { message = result.Error });

        return Ok(new { message = "Logout exitoso" });
    }

    [Authorize]
    [HttpPost("logout-all")]
    public async Task<IActionResult> LogoutAll()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
            return Unauthorized();

        var userId = int.Parse(userIdClaim.Value);

        var result = await _authService.LogoutAll(userId);

        if (!result.Success)
            return BadRequest(new { message = result.Error });

        return Ok(new { message = "Logout exitoso" });
    }
}