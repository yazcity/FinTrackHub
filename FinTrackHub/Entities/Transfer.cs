using FinTrackHub.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace FinTrackHub.Entities

{
    [Table("Tbl_Transfer")]
    public class Transfer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TranferId { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public long FromAccountId { get; set; }
        public long ToAccountId { get; set; }
        public string? Note { get; set; }
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

        [ForeignKey("FromAccountId")]
        public Account FromAccount { get; set; }
        [ForeignKey("ToAccountId")]
        public Account ToAccount { get; set; }
    }
}
