using APISegura.Dtos.Auth;
using APISegura.Dtos.Users;
using APISegura.Services;
using APISegura.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APISegura.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize]
    public class UsersController : Controller
    {
        private readonly IUserService _service;
        private readonly IAuthService _authService;

        public UsersController(
            IUserService service,
            IAuthService authService)
        {
            _service = service;
            _authService = authService;
        }

        [HttpGet("paged")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetPaged(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? filtro = null)
        {
            var result = await _service.GetPagedAsync(
                pageNumber,
                pageSize,
                filtro);

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);

            if (result == null)
                return NotFound();

            return Json(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(RegisterRequest request)
        {
            var result = await _authService.Register(request);

            if (!result.Success)
                return BadRequest(new { message = result.Error });

            return Ok(new { message = "Usuario creado" });
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(
            int id,
            UpdateUserRequest dto)
        {
            var usuario = User.Identity?.Name ?? "Sistema";

            var result = await _service.UpdateAsync(
                id,
                dto,
                usuario);

            if (!result)
                return Json(new
                {
                    success = false,
                    message = "No fue posible actualizar el usuario."
                });

            return Json(new
            {
                success = true
            });
        }

        [HttpPatch("{id:int}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SetActive(int id, SetUserStatusRequest dto)
        {
            var usuario = User.Identity?.Name ?? "Sistema";

            var result = await _service.SetActiveAsync(id, dto.Active, usuario);

            if (!result.IsSuccess)
            {
                return Json(new
                {
                    success = false,
                    message = result.Message
                });
            }

            return Json(new
            {
                success = true,
                message = result.Message
            });
        }
    }
}