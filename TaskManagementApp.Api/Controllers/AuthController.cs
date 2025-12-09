using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskManagementApp.Services;
using TaskManagementApp.Services.Exceptions;
using System;

namespace TaskManagementApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;
        public AuthController(IAuthService auth) { _auth = auth; }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var token = await _auth.LoginAsync(dto.Username, dto.Password);
            if (token == null)
                return Unauthorized(new { error = "Invalid username or password" });
            return Ok(new { token, username = dto.Username });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] LoginDto dto)
        {
            try
            {
                await _auth.RegisterAsync(dto.Username, dto.Password);
                return Ok();
            }
            catch (UsernameTakenException ex)
            {
                return Conflict(new { error = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        public class LoginDto { public string Username { get; set; } = null!; public string Password { get; set; } = null!; }
    }
}
