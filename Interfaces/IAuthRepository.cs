using FinTrackHub.Identity;
using FinTrackHub.Models.DTOs;
using Microsoft.AspNetCore.Identity;

namespace FinTrackHub.Interfaces
{
    public interface IAuthRepository
    {
        Task<bool> RegisterAsync(Register model);
        Task<string> LoginAsync(Login model);

        Task<ApplicationUser> FindByEmailAsync(string email);
        Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password);
        Task<bool> RoleExistsAsync(string role);
        Task<IdentityResult> CreateRoleAsync(ApplicationRole role);
        Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role);
        Task<SignInResult> PasswordSignInAsync(ApplicationUser user, string password, bool rememberMe);
    }
}
