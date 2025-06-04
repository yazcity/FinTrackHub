using FinTrackHub.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FinTrackHub.Entities
{
    [Table("Tbl_IncomeExpenseCategoryType")]
    public class IncomeExpenseCategoryType
    {
        //[key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long TypeId { get; set; }
        [Required]
        [MaxLength(250)]
        public string TypeName { get; set; }
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

        public ICollection<IncomeExpenseCategory> IncomeExpenseCategories { get; set; }
    }
}
