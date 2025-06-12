using FinTrackHub.Data;
using FinTrackHub.Entities;
using FinTrackHub.Interfaces;
using FinTrackHub.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace FinTrackHub.Repositories
{
    public class AccountGroupRepository : IAccountGroupRepository
    {
        private readonly ApplicationDbContext _context;

        public AccountGroupRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AccountGroupViewModel>> GetGroupsByUserIdAsync(string userId)
        {
            return await _context.AccountGroups
                .Where(x => x.IsActive && x.CreatedById == userId)
                .OrderBy(x => x.AccountgroupId)
                .Select(x => new AccountGroupViewModel
                {
                    AccountgroupId = x.AccountgroupId,
                    AccountGroupName = x.AccountGroupName,
                    AccountGroupTypeName = x.AccountGroupType.AccountGroupTypeName,
                    IsActive = x.IsActive,
                    CreatedDate = x.CreatedDate,
                    UpdatedDate = x.UpdatedDate
                })
                .ToListAsync();
        }

        public async Task<AccountGroupViewModel> GetByIdAsync(long id)
        {
            return await _context.AccountGroups
                .Where(x => x.AccountgroupId == id)
                .Select(x => new AccountGroupViewModel
                {
                    AccountgroupId = x.AccountgroupId,
                    AccountGroupName = x.AccountGroupName,
                    AccountGroupTypeName = x.AccountGroupType.AccountGroupTypeName,
                    IsActive = x.IsActive,
                    CreatedDate = x.CreatedDate,
                    UpdatedDate = x.UpdatedDate
                })
                .FirstOrDefaultAsync();
        }

        public async Task<ResponseResult> AddOrUpdateAsync(AccountGroupViewModel model, string userId)
        {
            if (model == null) return new ResponseResult(false, "Invalid data");

            var existingGroup = await _context.AccountGroups.FirstOrDefaultAsync(x => x.AccountgroupId == model.AccountgroupId);

            if (existingGroup == null)
            {
                var newGroup = new AccountGroup
                {
                    AccountGroupName = model.AccountGroupName,
                    AccountgroupTypeId = model.AccountgroupTypeId,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow,
                    CreatedById = userId,
                    UpdatedById = userId
                };

                _context.AccountGroups.Add(newGroup);
            }
            else
            {
                existingGroup.AccountGroupName = model.AccountGroupName;
                existingGroup.AccountgroupTypeId = model.AccountgroupTypeId;
                existingGroup.UpdatedDate = DateTime.UtcNow;
                existingGroup.UpdatedById = userId;

                _context.AccountGroups.Update(existingGroup);
            }

            await _context.SaveChangesAsync();
            return new ResponseResult(true, "Account group saved successfully");
        }

        public async Task<ResponseResult> DeleteAsync(long id, string userId)
        {
            var accountGroup = await _context.AccountGroups.FirstOrDefaultAsync(x => x.AccountgroupId == id);
            if (accountGroup == null) return new ResponseResult(false, "Account group not found");

            var isUsed = await _context.Accounts.AnyAsync(x => x.AccountgroupId == id);
            if (isUsed) return new ResponseResult(false, "Account group is in use");

            accountGroup.IsActive = false;
            accountGroup.UpdatedDate = DateTime.UtcNow;
            accountGroup.UpdatedById = userId;

            _context.AccountGroups.Update(accountGroup);
            await _context.SaveChangesAsync();

            return new ResponseResult(true, "Account group deleted successfully");
        }
    }


}
