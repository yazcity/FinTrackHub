using FinTrackHub.Common;
using FinTrackHub.Data;
using FinTrackHub.Entities;
using FinTrackHub.Extensions;
using FinTrackHub.Interfaces;
using FinTrackHub.Models.DTOs;
using FinTrackHub.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FinTrackHub.Repositories
{
    public class TransactionCategoryRepository : ITransactionCategoryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly List<long> categoryType = CategoryType.All;

        public TransactionCategoryRepository(ApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Result<IEnumerable<TransactionCategoryDto>>> GetAllCategoriesAsync()
        {
            try
            {
                var userId = _currentUserService.UserId;
                if (string.IsNullOrEmpty(userId))
                    return Result<IEnumerable<TransactionCategoryDto>>.Failure("User is not authenticated");

                var categories = await _context.IncomeExpenseCategories
                    .Include(c => c.IncomeExpenseCategoryType)
                    .Where(x => categoryType.Contains(x.TypeId) && x.IsActive && x.CreatedById == userId)
                    .ToListAsync();

                var result = categories.Select(c => c.MapToDto()).ToList();
                return Result<IEnumerable<TransactionCategoryDto>>.Success(result, "Categories retrieved successfully");
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<TransactionCategoryDto>>.Failure($"An error occurred: {ex.Message}");
            }
        }

        public async Task<Result<TransactionCategoryDto>> GetCategoryByIdAsync(long id)
        {
            try
            {
                var userId = _currentUserService.UserId;
                if (string.IsNullOrEmpty(userId))
                    return Result<TransactionCategoryDto>.Failure("User is not authenticated");

                var category = await _context.IncomeExpenseCategories
                    .Include(c => c.IncomeExpenseCategoryType)
                    .FirstOrDefaultAsync(gt => gt.CategoryId == id && gt.IsActive && gt.CreatedById == userId);

                if (category == null)
                    return Result<TransactionCategoryDto>.Failure("Category not found");

                return Result<TransactionCategoryDto>.Success(category.MapToDto(), "Category retrieved successfully");
            }
            catch (Exception ex)
            {
                return Result<TransactionCategoryDto>.Failure($"An error occurred: {ex.Message}");
            }
        }

        public async Task<Result<TransactionCategoryDto>> SaveCategoryAsync(TransactionCategoryDto model)
        {
            try
            {
                var userId = _currentUserService.UserId;
                if (string.IsNullOrEmpty(userId))
                    return Result<TransactionCategoryDto>.Failure("User is not authenticated");

                if (!categoryType.Contains(model.TypeId))
                    return Result<TransactionCategoryDto>.Failure("Invalid category type");

                IncomeExpenseCategory entity;
                bool isNew = model.CategoryId == 0;

                if (isNew)
                {
                    entity = new IncomeExpenseCategory
                    {
                        CategoryName = model.CategoryName,
                        TypeId = model.TypeId,
                        IsActive = true,
                        CreatedDate = DateTime.UtcNow,
                        UpdatedDate = DateTime.UtcNow,
                        CreatedById = userId,
                        UpdatedById = userId
                    };

                    _context.IncomeExpenseCategories.Add(entity);
                }
                else
                {
                    entity = await _context.IncomeExpenseCategories
                        .Include(c => c.IncomeExpenseCategoryType)
                        .FirstOrDefaultAsync(x => x.CategoryId == model.CategoryId && x.CreatedById == userId);

                    if (entity == null)
                        return Result<TransactionCategoryDto>.Failure("Category not found");

                    entity.CategoryName = model.CategoryName;
                    entity.TypeId = model.TypeId;
                    entity.UpdatedDate = DateTime.UtcNow;
                    entity.UpdatedById = userId;
                }

                await _context.SaveChangesAsync();

                if (isNew)
                {
                    entity = await _context.IncomeExpenseCategories
                        .Include(c => c.IncomeExpenseCategoryType)
                        .FirstOrDefaultAsync(x => x.CategoryId == entity.CategoryId);
                }

                return Result<TransactionCategoryDto>.Success(entity!.MapToDto(), isNew ? "Category added successfully" : "Category updated successfully");
            }
            catch (Exception ex)
            {
                return Result<TransactionCategoryDto>.Failure($"An error occurred: {ex.Message}");
            }
        }

        public async Task<Result<string>> DeleteCategoryAsync(long id)
        {
            try
            {
                var userId = _currentUserService.UserId;
                if (string.IsNullOrEmpty(userId))
                    return Result<string>.Failure("User is not authenticated");

                var category = await _context.IncomeExpenseCategories
                    .FirstOrDefaultAsync(x => x.CategoryId == id && x.CreatedById == userId && x.IsActive);

                if (category == null)
                    return Result<string>.Failure("Category not found");

                category.IsActive = false;
                category.UpdatedDate = DateTime.UtcNow;
                category.UpdatedById = userId;

                _context.IncomeExpenseCategories.Update(category);
                await _context.SaveChangesAsync();

                return Result<string>.Success("Category deleted successfully", "Category deleted successfully");
            }
            catch (Exception ex)
            {
                return Result<string>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }


}
