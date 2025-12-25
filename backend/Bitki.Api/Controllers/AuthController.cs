using Bitki.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bitki.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _authService.LoginAsync(request.Username, request.Password);

            if (result == null)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            return Ok(new
            {
                token = result.Value.Token,
                role = result.Value.Role,
                username = result.Value.Username
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await _authService.RegisterAsync(request.Username, request.Password);

            if (!result)
            {
                return BadRequest(new { message = "Username already exists" });
            }

            return Ok(new { message = "Registration successful" });
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
