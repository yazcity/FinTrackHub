using Microsoft.AspNetCore.Identity;

namespace FinTrackHub.Identity
{
    public class ApplicationUser: IdentityUser
    {
        public string? FullName { get; set; }
        public string? Address { get; set; }
        public string? PostalCode { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
    }
}
