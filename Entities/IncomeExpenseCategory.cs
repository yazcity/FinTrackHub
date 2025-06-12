using FinTrackHub.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FinTrackHub.Entities
{
    [Table("Tbl_IncomeExpenseCategory")]
    public class IncomeExpenseCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CategoryId { get; set; }

        public long TypeId { get; set; }

        [Required]
        [MaxLength(250)]
        public string CategoryName { get; set; }
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

        [ForeignKey("TypeId")]
        public virtual IncomeExpenseCategoryType IncomeExpenseCategoryType { get; set; }

        public ICollection<Transaction> Transactions { get; set; }
        public ICollection<Budget>? Budget { get; set; }
    }
}
