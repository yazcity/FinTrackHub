using FinTrackHub.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FinTrackHub.Entities
{
    [Table("Tbl_AccountGroupType")]
    public class AccountGroupType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AccountgroupTypeId { get; set; }
        [MaxLength(250)]
        [Required(ErrorMessage = "Type Name is required.")]
        [StringLength(250, ErrorMessage = "Type Name cannot be longer than 250 characters.")]
        public string AccountGroupTypeName { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeletable { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
        public string? CreatedById { get; set; } // Foreign Key for CreatedBy
        public string? UpdatedById { get; set; } // Foreign Key for UpdatedBy

        // Navigation Properties
        [ForeignKey("CreatedById")]
        public virtual ApplicationUser CreatedByUser { get; set; }

        [ForeignKey("UpdatedById")]
        public virtual ApplicationUser UpdatedByUser { get; set; }

        // Navigation property for related AccountGroup records
        public ICollection<AccountGroup> AccountGroups { get; set; }
    }
}
