using FinTrackHub.Entities;
using FinTrackHub.Services.Interfaces;
using FinTrackHub.Data;
using FinTrackHub.Interfaces;
using FinTrackHub.Models.DTOs;

namespace FinTrackHub.Services
{
    public class AccountGroupService : IAccountGroupService
    {
        private readonly IAccountGroupRepository _accountGroupRepository;

        public AccountGroupService(IAccountGroupRepository accountGroupRepository)
        {
            _accountGroupRepository = accountGroupRepository;
        }

        public async Task<IEnumerable<AccountGroupViewModel>> GetGroupsByUserIdAsync(string userId)
        {
            return await _accountGroupRepository.GetGroupsByUserIdAsync(userId);
        }

        public async Task<AccountGroupViewModel> GetByIdAsync(long id)
        {
            return await _accountGroupRepository.GetByIdAsync(id);
        }

        public async Task<ResponseResult> AddOrUpdateAsync(AccountGroupViewModel model, string userId)
        {
            return await _accountGroupRepository.AddOrUpdateAsync(model, userId);
        }

        public async Task<ResponseResult> DeleteAsync(long id, string userId)
        {
            return await _accountGroupRepository.DeleteAsync(id, userId);
        }
    }


}
