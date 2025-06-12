using FinTrackHub.Entities;
using FinTrackHub.Models.DTOs;

namespace FinTrackHub.Interfaces
{
    public interface IAccountGroupRepository
    {
        Task<IEnumerable<AccountGroupViewModel>> GetGroupsByUserIdAsync(string userId);
        Task<AccountGroupViewModel> GetByIdAsync(long id);
        Task<ResponseResult> AddOrUpdateAsync(AccountGroupViewModel model, string userId);
        Task<ResponseResult> DeleteAsync(long id, string userId);
    }

}
