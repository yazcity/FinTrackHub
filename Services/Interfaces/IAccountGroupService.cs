using FinTrackHub.Models.DTOs;

namespace FinTrackHub.Services.Interfaces
{
    public interface IAccountGroupService
    {
        Task<IEnumerable<AccountGroupViewModel>> GetGroupsByUserIdAsync(string userId);
        Task<AccountGroupViewModel> GetByIdAsync(long id);
        Task<ResponseResult> AddOrUpdateAsync(AccountGroupViewModel model, string userId);
        Task<ResponseResult> DeleteAsync(long id, string userId);
    }


}
