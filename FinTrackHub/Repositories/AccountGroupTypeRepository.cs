using FinTrackHub.Data;
using FinTrackHub.Entities;
using FinTrackHub.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinTrackHub.Repositories
{
    public class AccountGroupTypeRepository : IAccountGroupTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public AccountGroupTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AccountGroupType>> GetGroupTypesAsync(string userId)
        {
            return await _context.AccountGroupTypes
                .Where(x => x.IsActive && (x.CreatedById == userId || !x.IsDeletable))
                .ToListAsync();
        }

        public async Task<AccountGroupType> GetByIdAsync(long id)
        {
            return await _context.AccountGroupTypes.FindAsync(id);
        }

        public async Task AddAsync(AccountGroupType accountGroupType)
        {
            await _context.AccountGroupTypes.AddAsync(accountGroupType);
        }

        public async Task UpdateAsync(AccountGroupType accountGroupType)
        {
            _context.AccountGroupTypes.Update(accountGroupType);
        }

        public async Task<bool> ExistsInAccountGroupsAsync(long id)
        {
            return await _context.AccountGroups.AnyAsync(x => x.AccountgroupTypeId == id);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }



}
