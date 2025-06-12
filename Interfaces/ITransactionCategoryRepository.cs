using FinTrackHub.Common;
using FinTrackHub.Entities;
using FinTrackHub.Models.DTOs;

namespace FinTrackHub.Interfaces
{
    public interface ITransactionCategoryRepository
    {
        Task<Result<IEnumerable<TransactionCategoryDto>>> GetAllCategoriesAsync();
        Task<Result<TransactionCategoryDto>> GetCategoryByIdAsync(long id);
        Task<Result<TransactionCategoryDto>> SaveCategoryAsync(TransactionCategoryDto model);
        Task<Result<string>> DeleteCategoryAsync(long id);
    }

}
