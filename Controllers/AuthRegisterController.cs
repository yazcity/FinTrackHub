using FinTrackHub.Common;
using FinTrackHub.Models.DTOs;
using FinTrackHub.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace FinTrackHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthRegisterController : BaseController
    {
        private readonly AuthService _authService;

        public AuthRegisterController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Register model)
        {
            if (!ModelState.IsValid)
                return BadRequestResponse(GetModelErrors());

            var result = await _authService.RegisterUserAsync(model);

            if (!result)
                return BadRequestResponse("Registration failed!");

            return OkResponse<string>(null, "Registration successful!");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login model)
        {
            if (!ModelState.IsValid)
                return BadRequestResponse(GetModelErrors());

            var token = await _authService.LoginUserAsync(model);

            if (token == null)
                return UnauthorizedResponse("Invalid credentials!");

            return OkResponse(token, "Login successful!");
        }

        private string GetModelErrors()
        {
            return string.Join(", ",
                ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        }
    }
}

