using FinTrackHub.Identity;
using FinTrackHub.Interfaces;
using FinTrackHub.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FinTrackHub.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthRepository(UserManager<ApplicationUser> userManager,
                              SignInManager<ApplicationUser> signInManager,
                              RoleManager<ApplicationRole> roleManager,
                              IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<bool> RegisterAsync(Register model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.Name
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return false;

            // Assign default role "User"
            if (!await _roleManager.RoleExistsAsync("User"))
            {
                var role = new ApplicationRole { Name = "User", Description = "Regular user role" };
                await _roleManager.CreateAsync(role);
            }
            await _userManager.AddToRoleAsync(user, "User");

            return true;
        }

        public async Task<string> LoginAsync(Login model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) return null;

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
            return result.Succeeded ? GenerateJwtToken(user) : null;
        }

        private string GenerateJwtToken(ApplicationUser user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.NameId, user.UserName),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, "User")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:ExpirationMinutes"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



        public async Task<ApplicationUser> FindByEmailAsync(string email) => await _userManager.FindByEmailAsync(email);

        public async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password) => await _userManager.CreateAsync(user, password);

        public async Task<bool> RoleExistsAsync(string role) => await _roleManager.RoleExistsAsync(role);

        public async Task<IdentityResult> CreateRoleAsync(ApplicationRole role) => await _roleManager.CreateAsync(role);

        public async Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role) => await _userManager.AddToRoleAsync(user, role);

        public async Task<SignInResult> PasswordSignInAsync(ApplicationUser user, string password, bool rememberMe) =>
            await _signInManager.PasswordSignInAsync(user, password, rememberMe, lockoutOnFailure: false);
    }

}
