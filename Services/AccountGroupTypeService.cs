using FinTrackHub.Entities;
using FinTrackHub.Interfaces;
using FinTrackHub.Models.DTOs;
using FinTrackHub.Services.Interfaces;

namespace FinTrackHub.Services
{
    public class AccountGroupTypeService : IAccountGroupTypeService
    {
        private readonly IAccountGroupTypeRepository _repository;

        public AccountGroupTypeService(IAccountGroupTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<AccountGroupTypeViewModel>> GetGroupTypesAsync(string userId)
        {
            var groupTypes = await _repository.GetGroupTypesAsync(userId);

            return groupTypes.Select(gt => new AccountGroupTypeViewModel
            {
                AccountgroupTypeId = gt.AccountgroupTypeId,
                AccountGroupTypeName = gt.AccountGroupTypeName,
                IsActive = gt.IsActive,
                CreatedDate = gt.CreatedDate,
                UpdatedDate = gt.UpdatedDate,
                IsDeletable = gt.IsDeletable
            }).ToList();
        }

        public async Task<AccountGroupTypeViewModel> GetByIdAsync(long id)
        {
            var groupType = await _repository.GetByIdAsync(id);
            if (groupType == null)
            {
                return null;
            }

            return new AccountGroupTypeViewModel
            {
                AccountgroupTypeId = groupType.AccountgroupTypeId,
                AccountGroupTypeName = groupType.AccountGroupTypeName
            };
        }

        public async Task AddOrUpdateAsync(AccountGroupTypeViewModel model, string userId)
        {
            AccountGroupType accountGroupType;

            if (model.AccountgroupTypeId == 0)
            {
                accountGroupType = new AccountGroupType
                {
                    AccountGroupTypeName = model.AccountGroupTypeName,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow,
                    CreatedById = userId,
                    UpdatedById = userId
                };

                await _repository.AddAsync(accountGroupType);
            }
            else
            {
                accountGroupType = await _repository.GetByIdAsync(model.AccountgroupTypeId);
                if (accountGroupType == null)
                {
                    throw new Exception("Account Group Type not found.");
                }

                accountGroupType.AccountGroupTypeName = model.AccountGroupTypeName;
                accountGroupType.UpdatedDate = DateTime.UtcNow;
                accountGroupType.UpdatedById = userId;

                await _repository.UpdateAsync(accountGroupType);
            }

            await _repository.SaveAsync();
        }

        public async Task<bool> DeleteAsync(long id, string userId)
        {
            var used = await _repository.ExistsInAccountGroupsAsync(id);
            if (used)
            {
                return false; // Can't delete if it's being used in account groups
            }

            var accountGroupType = await _repository.GetByIdAsync(id);
            if (accountGroupType == null)
            {
                return false;
            }

            accountGroupType.IsActive = false;
            accountGroupType.UpdatedDate = DateTime.UtcNow;
            accountGroupType.UpdatedById = userId;

            await _repository.UpdateAsync(accountGroupType);
            await _repository.SaveAsync();

            return true;
        }
    }

}
