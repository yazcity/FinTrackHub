using FinTrackHub.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FinTrackHub.Entities
{
    [Table("Tbl_Transaction")]
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TransactionId { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public long? CategoryId { get; set; }
        public long? AccountId { get; set; }
        public string? Note { get; set; }
        public string? Description { get; set; }
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

        [ForeignKey("CategoryId")]
        public IncomeExpenseCategory? IncomeExpenseCategory { get; set; }
        [ForeignKey("AccountId")]
        public Account? Account { get; set; }

    }
}
