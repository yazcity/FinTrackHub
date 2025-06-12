using FinTrackHub.Models.DTOs;

namespace FinTrackHub.Services.Interfaces
{
    public interface IAccountGroupTypeService
    {
        Task<IEnumerable<AccountGroupTypeViewModel>> GetGroupTypesAsync(string userId);
        Task<AccountGroupTypeViewModel> GetByIdAsync(long id);
        Task AddOrUpdateAsync(AccountGroupTypeViewModel model, string userId);
        Task<bool> DeleteAsync(long id, string userId);
    }

}
