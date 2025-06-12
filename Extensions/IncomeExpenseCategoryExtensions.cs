using FinTrackHub.Entities;
using FinTrackHub.Models.DTOs;

namespace FinTrackHub.Extensions
{
    public static class IncomeExpenseCategoryExtensions
    {
        public static TransactionCategoryDto MapToDto(this IncomeExpenseCategory category)
        {
            return new TransactionCategoryDto
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                TypeId = category.TypeId,
                TypeName = category.IncomeExpenseCategoryType?.TypeName ?? string.Empty,
                CreatedDate = category.CreatedDate,
                UpdatedDate = category.UpdatedDate
            };
        }
    }
}
