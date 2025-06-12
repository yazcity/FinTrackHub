using FinTrackHub.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FinTrackHub.Entities
{
    [Table("Tbl_AccountGroup")]
    public class AccountGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AccountgroupId { get; set; }
        public long AccountgroupTypeId { get; set; }
        [Required]
        [MaxLength(250)]
        public string AccountGroupName { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
        public string? CreatedById { get; set; } // Foreign Key for CreatedBy
        public string? UpdatedById { get; set; } // Foreign Key for UpdatedBy

        // Navigation Properties
        [ForeignKey("CreatedById")]
        public virtual ApplicationUser CreatedByUser { get; set; }

        [ForeignKey("UpdatedById")]
        public virtual ApplicationUser UpdatedByUser { get; set; }

        [ForeignKey("AccountgroupTypeId")]
        public virtual AccountGroupType AccountGroupType { get; set; }

        public ICollection<Account> Accounts { get; set; }
    }
}
