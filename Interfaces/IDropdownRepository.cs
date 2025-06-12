using FinTrackHub.Common;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FinTrackHub.Interfaces
{
    public interface IDropdownRepository
    {
        Task<Result<List<SelectListItem>>> GetAccountGroupTypesAsync();
        Task<Result<List<SelectListItem>>> GetAccountGroupsAsync(long accountGroupTypeId);
        Task<Result<List<SelectListItem>>> GetIncomeExpenseCategoryTypesAsync();
        Task<Result<List<SelectListItem>>> GetIncomeExpenseCategoriesAsync(long typeId);
        Task<Result<List<SelectListItem>>> GetAccountsAsync();
    }
}
