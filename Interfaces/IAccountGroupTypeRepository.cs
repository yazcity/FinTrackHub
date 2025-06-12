using FinTrackHub.Entities;

namespace FinTrackHub.Interfaces
{
    public interface IAccountGroupTypeRepository
    {
        Task<IEnumerable<AccountGroupType>> GetGroupTypesAsync(string userId);
        Task<AccountGroupType> GetByIdAsync(long id);
        Task AddAsync(AccountGroupType accountGroupType);
        Task UpdateAsync(AccountGroupType accountGroupType);
        Task<bool> ExistsInAccountGroupsAsync(long id);
        Task SaveAsync();
    }

}
