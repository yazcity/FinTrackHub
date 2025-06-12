using FinTrackHub.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FinTrackHub.Entities
{
    [Table("Tbl_Budget")]
    public class Budget
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long BudgetId { get; set; } // Primary Key
        public decimal Amount { get; set; } // Budgeted amount
        public long CategoryId { get; set; } // Link to IncomeExpenseCategory
        public long? AccountId { get; set; } // Optional: Link to a specific account
        public DateTime? StartDate { get; set; } // Budget start date
        public DateTime? EndDate { get; set; } // Budget end date
        public string? Note { get; set; } // Optional notes
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

        // Navigation properties
        [ForeignKey("CategoryId")]
        public IncomeExpenseCategory IncomeExpenseCategory { get; set; }
        [ForeignKey("AccountId")]
        public Account? Account { get; set; }
    }
}
