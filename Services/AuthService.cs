using FinTrackHub.Interfaces;
using FinTrackHub.Models.DTOs;

namespace FinTrackHub.Services
{
    public class AuthService
    {
        private readonly IAuthRepository _authRepository;

        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task<bool> RegisterUserAsync(Register model)
        {
            return await _authRepository.RegisterAsync(model);
        }

        public async Task<string> LoginUserAsync(Login model)
        {
            return await _authRepository.LoginAsync(model);
        }
    }
}
