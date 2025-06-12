using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace FinTrackHub.Models.DTOs
{
    public class AccountGroupViewModel
    {
        public AccountGroupViewModel()
        {
            AccountGroupTypeOptions = new List<SelectListItem>();
        }
        public long AccountgroupId { get; set; }
        public long AccountgroupTypeId { get; set; }
        [MaxLength(250)]
        [Required(ErrorMessage = "Type Name is required.")]
        [StringLength(250, ErrorMessage = "Type Name cannot be longer than 250 characters.")]
        public string AccountGroupName { get; set; }
        public string AccountGroupTypeName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }


        public List<SelectListItem> AccountGroupTypeOptions { get; set; }




    }
}
