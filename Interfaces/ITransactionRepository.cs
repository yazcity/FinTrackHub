using FinTrackHub.Common;
using FinTrackHub.Entities;


namespace FinTrackHub.Interfaces
{
    public interface ITransactionRepository
    {
        Task<Result<Transaction>> AddTransactionAsync(Transaction transaction);
        Task<Result<bool>> DeleteTransactionAsync(long transactionId, string updatedById);
        Task<Result<Transaction>> GetTransactionByIdAsync(long transactionId);
        Task<Result<IEnumerable<Transaction>>> GetAllTransactionsAsync(string? userId);
        Task<Result<Transaction>> UpdateTransactionAsync(Transaction transaction);
        Task<Result<Transaction>> StageUpdateTransactionAsync(Transaction transaction);
        Task<Result<bool>> TransferFundsAsync(long sourceAccountId, long destinationAccountId, decimal amount, string createdById);
        Task<Result<bool>> DeleteTransactionsByAccountIdAsync(long accountId, string updatedById);
        Task<Result<bool>> DeleteTransactionsAsync(long transactionId, string updatedById);
        Task<Result<bool>> HasTransactionsByAccountIdAsync(long accountId);
    }

}
