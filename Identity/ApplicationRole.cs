using Microsoft.AspNetCore.Identity;

namespace FinTrackHub.Identity
{
    public class ApplicationRole: IdentityRole
    {
        public string? Description { get; set; }

        // Additional custom fields (e.g., creation date, etc.)
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
    }
}
