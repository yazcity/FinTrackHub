using FinTrackHub.Common;
using FinTrackHub.Entities;
using FinTrackHub.Models.DTOs;

namespace FinTrackHub.Interfaces
{

    public interface IAccountRepository
    {
        Task<Result<AccountDto>> GetAccountByIdAsync(long accountId);        // Read - Select by ID
        Task<Result<IEnumerable<AccountDto>>> GetAllAccountsAsync(string? UserId);        // Read - Select All

        Task<Result<AccountDto>> AddAccountAsync(AccountDto account);   // Create - Insert
        Task<Result<AccountDto>> UpdateAccountAsync(AccountDto account); // Update
        Task<Result<string>> DeleteAccountAsync(long accountId, string UpdatedById);  // Delete
    }

}
