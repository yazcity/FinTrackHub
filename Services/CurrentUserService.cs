using FinTrackHub.Services.Interfaces;
using System.Security.Claims;

namespace FinTrackHub.Services
{
    public class CurrentUserService: ICurrentUserService
    {
        public string? UserId { get; }

        public CurrentUserService(IHttpContextAccessor contextAccessor)
        {
            UserId = contextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }

    }
}
