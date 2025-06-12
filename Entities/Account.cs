using FinTrackHub.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FinTrackHub.Entities
{
    [Table("Tbl_Account")]
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AccountId { get; set; }
        public long AccountgroupId { get; set; }
        [Required]
        [MaxLength(250)]
        public string AccountName { get; set; }
        [Required]
        public decimal? Amount { get; set; }
        public decimal? Payable { get; set; }
        public bool IsIncludeInTotal { get; set; } = false;
        public string? Description { get; set; }
        public string? Note { get; set; }
        public string? Attachement { get; set; }
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

        [ForeignKey("AccountgroupId")]
        public virtual AccountGroup AccountGroup { get; set; }

        public ICollection<Transaction> Transactions { get; set; }
    }
}
