using FinTrackHub.Common;
using FinTrackHub.Data;
using FinTrackHub.Interfaces;
using FinTrackHub.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FinTrackHub.Repositories
{
    public class DropdownRepository : IDropdownRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public DropdownRepository(ApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Result<List<SelectListItem>>> GetAccountGroupTypesAsync()
        {
            try
            {
                var userId = _currentUserService.UserId;

                var list = await _context.AccountGroupTypes
                    .Where(x => x.IsActive && (x.CreatedById == userId || !x.IsDeletable))
                    .Select(x => new SelectListItem
                    {
                        Value = x.AccountgroupTypeId.ToString(),
                        Text = x.AccountGroupTypeName
                    })
                    .ToListAsync();

                return Result<List<SelectListItem>>.Success(list, "Account group types loaded successfully.");
            }
            catch (Exception ex)
            {
                return Result<List<SelectListItem>>.Failure($"Failed to load account group types: {ex.Message}");
            }
        }

        public async Task<Result<List<SelectListItem>>> GetAccountGroupsAsync(long accountGroupTypeId)
        {
            try
            {
                var userId = _currentUserService.UserId;

                var query = _context.AccountGroups
                    .Where(x => x.IsActive && x.CreatedById == userId);

                if (accountGroupTypeId != 0)
                    query = query.Where(x => x.AccountgroupTypeId == accountGroupTypeId);

                var list = await query
                    .Select(x => new SelectListItem
                    {
                        Value = x.AccountgroupId.ToString(),
                        Text = x.AccountGroupName
                    })
                    .ToListAsync();

                return Result<List<SelectListItem>>.Success(list, "Account groups loaded successfully.");
            }
            catch (Exception ex)
            {
                return Result<List<SelectListItem>>.Failure($"Failed to load account groups: {ex.Message}");
            }
        }

        public async Task<Result<List<SelectListItem>>> GetIncomeExpenseCategoryTypesAsync()
        {
            try
            {
                var list = await _context.IncomeExpenseCategoryTypes
                    .Where(x => x.IsActive)
                    .Select(x => new SelectListItem
                    {
                        Value = x.TypeId.ToString(),
                        Text = x.TypeName
                    })
                    .ToListAsync();

                return Result<List<SelectListItem>>.Success(list, "Category types loaded successfully.");
            }
            catch (Exception ex)
            {
                return Result<List<SelectListItem>>.Failure($"Failed to load category types: {ex.Message}");
            }
        }

        public async Task<Result<List<SelectListItem>>> GetIncomeExpenseCategoriesAsync(long typeId)
        {
            try
            {
                var userId = _currentUserService.UserId;

                var list = await _context.IncomeExpenseCategories
                    .Where(x => x.TypeId == typeId && x.IsActive && (x.CreatedById == userId || !x.IsDeletable))
                    .Select(x => new SelectListItem
                    {
                        Value = x.CategoryId.ToString(),
                        Text = x.CategoryName
                    })
                    .ToListAsync();

                return Result<List<SelectListItem>>.Success(list, "Categories loaded successfully.");
            }
            catch (Exception ex)
            {
                return Result<List<SelectListItem>>.Failure($"Failed to load categories: {ex.Message}");
            }
        }

        public async Task<Result<List<SelectListItem>>> GetAccountsAsync()
        {
            try
            {
                var userId = _currentUserService.UserId;

                var list = await _context.Accounts
                    .Where(x => x.IsActive && x.CreatedById == userId)
                    .Select(x => new SelectListItem
                    {
                        Value = x.AccountId.ToString(),
                        Text = x.AccountName
                    })
                    .ToListAsync();

                return Result<List<SelectListItem>>.Success(list, "Accounts loaded successfully.");
            }
            catch (Exception ex)
            {
                return Result<List<SelectListItem>>.Failure($"Failed to load accounts: {ex.Message}");
            }
        }
    }

}
