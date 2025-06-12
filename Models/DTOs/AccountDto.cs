namespace FinTrackHub.Models.DTOs
{
    public class AccountDto
    {
        public int? RowNumber { get; set; }
        public long AccountId { get; set; }
        public string AccountName { get; set; }
      //  public AccountGroupDto? AccountGroup { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Payable { get; set; }
        public decimal? CurrentBalance { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public long AccountgroupId { get; set; }
        public string? Description { get; set; }
        public string? Note { get; set; }
        public bool IsIncludeInTotal { get; set; }
        public string? AccountGroupName { get; set; }
        public string? AccountGroupTypeName { get; set; }
        public string? Attachement { get; set; }
        public bool IsActive { get; set; }
    }
}
