using System.ComponentModel.DataAnnotations;

namespace FinTrackHub.Models.DTOs
{
    public class TransactionCategoryDto
    {
        public long CategoryId { get; set; }
        [Required(ErrorMessage = "Type is required.")]
        public long TypeId { get; set; }
        [MaxLength(250)]
        [Required(ErrorMessage = "Type Name is required.")]
        [StringLength(250, ErrorMessage = "Type Name cannot be longer than 250 characters.")]
        public string CategoryName { get; set; }
        public string TypeName { get; set; }
        public DateTime CreatedDate  { get; set; }
        public DateTime UpdatedDate  { get; set; }
    }
}
