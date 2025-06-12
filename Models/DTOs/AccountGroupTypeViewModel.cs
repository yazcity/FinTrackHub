using System.ComponentModel.DataAnnotations;

namespace FinTrackHub.Models.DTOs
{
    public class AccountGroupTypeViewModel
    {
        public long AccountgroupTypeId { get; set; }
        [MaxLength(250)]
        [Required(ErrorMessage = "Type Name is required.")]
        [StringLength(250, ErrorMessage = "Type Name cannot be longer than 250 characters.")]
        public string AccountGroupTypeName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsDeletable { get; set; }
    }
}
