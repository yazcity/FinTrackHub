using FinTrackHub.Models;
using FinTrackHub.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinTrackHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthRegisterController : ControllerBase
    {
        private readonly AuthService _authService;
        public AuthRegisterController(AuthService authService)
        {
            this._authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Register model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterUserAsync(model);
            if (!result)
                return BadRequest(new { Message = "Registration failed!" });

            return Ok(new { Message = "Registration successful!" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var token = await _authService.LoginUserAsync(model);
            if (token == null)
                return Unauthorized(new { Message = "Invalid credentials!" });

            return Ok(new { Token = token });
        }
    }
}
