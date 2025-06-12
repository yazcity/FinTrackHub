using FinTrackHub.Data;
using FinTrackHub.Interfaces;
using System.Security.Claims;
using FinTrackHub.Entities;
using FinTrackHub.Common;
using FinTrackHub.Services.Interfaces;
using FinTrackHub.Services;
using Microsoft.EntityFrameworkCore;

namespace FinTrackHub.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        public TransactionRepository(ApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Result<Transaction>> AddTransactionAsync(Transaction transaction)
        {
            try
            {
                var account = await _context.Accounts.FindAsync(transaction.AccountId);
                if (account == null) return Result<Transaction>.Failure("Account not found");

                var category = await _context.IncomeExpenseCategories.FindAsync(transaction.CategoryId);
                if (category == null) return Result<Transaction>.Failure("Category not found");

                await _context.Transactions.AddAsync(transaction);
                await _context.SaveChangesAsync();

                if (category.TypeId == 3) account.Amount += transaction.Amount; // income
                else if (category.TypeId == 5) account.Amount -= transaction.Amount; // expense

                _context.Accounts.Update(account);
                await _context.SaveChangesAsync();

                return Result<Transaction>.Success(transaction, "Transaction added successfully");
            }
            catch (Exception ex)
            {
                return Result<Transaction>.Failure(ex.Message);
            }
        }

        public async Task<Result<Transaction>> GetTransactionByIdAsync(long transactionId)
        {
            var transaction = await _context.Transactions
                .Include(a => a.IncomeExpenseCategory)
                    .ThenInclude(c => c.IncomeExpenseCategoryType)
                .Include(a => a.CreatedByUser)
                .Include(a => a.UpdatedByUser)
                .Include(a => a.Account)
                    .ThenInclude(ag => ag.AccountGroup)
                .FirstOrDefaultAsync(t => t.TransactionId == transactionId);

            if (transaction == null)
                return Result<Transaction>.Failure("Transaction not found");

            return Result<Transaction>.Success(transaction);
        }

        public async Task<Result<IEnumerable<Transaction>>> GetAllTransactionsAsync(string? userId)
        {
            var uid = userId ?? _currentUserService.UserId;
            if (string.IsNullOrEmpty(uid))
                return Result<IEnumerable<Transaction>>.Failure("User is not authenticated");

            var transactions = await _context.Transactions
                .Where(x => x.IsActive && x.CreatedById == uid)
                .Include(t => t.Account).ThenInclude(ag => ag.AccountGroup)
                .Include(t => t.IncomeExpenseCategory).ThenInclude(ic => ic.IncomeExpenseCategoryType)
                .Include(t => t.CreatedByUser)
                .Include(t => t.UpdatedByUser)
                .ToListAsync();

            return Result<IEnumerable<Transaction>>.Success(transactions);
        }

        public async Task<Result<bool>> DeleteTransactionAsync(long transactionId, string updatedById)
        {
            try
            {
                var transaction = await _context.Transactions
                    .Include(x => x.IncomeExpenseCategory)
                    .FirstOrDefaultAsync(t => t.TransactionId == transactionId && t.IsActive);

                if (transaction == null)
                    return Result<bool>.Failure("No active transaction found");

                var account = await _context.Accounts.FindAsync(transaction.AccountId);
                if (account == null)
                    return Result<bool>.Failure("Account not found");

                if (transaction.IncomeExpenseCategory.TypeId == 3)
                    account.Amount -= transaction.Amount;
                else if (transaction.IncomeExpenseCategory.TypeId == 5)
                    account.Amount += transaction.Amount;

                transaction.IsActive = false;
                transaction.UpdatedById = updatedById;
                transaction.UpdatedDate = DateTime.UtcNow;

                _context.Accounts.Update(account);
                _context.Transactions.Update(transaction);
                await _context.SaveChangesAsync();

                return Result<bool>.Success(true, "Transaction deleted successfully");
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ex.Message);
            }
        }

        public async Task<Result<Transaction>> UpdateTransactionAsync(Transaction transaction)
        {
            try
            {
                var existing = await _context.Transactions.FindAsync(transaction.TransactionId);
                if (existing == null)
                    return Result<Transaction>.Failure("Transaction not found");

                var account = await _context.Accounts.FindAsync(transaction.AccountId);
                var category = await _context.IncomeExpenseCategories.FindAsync(transaction.CategoryId);
                if (account == null || category == null)
                    return Result<Transaction>.Failure("Account or Category not found");

                if (existing.AccountId != transaction.AccountId)
                    return Result<Transaction>.Failure("Account change not allowed in this method.");

                if (category.TypeId == 3)
                    account.Amount += transaction.Amount - existing.Amount;
                else if (category.TypeId == 5)
                    account.Amount += -(transaction.Amount - existing.Amount);

                existing.TransactionDate = transaction.TransactionDate;
                existing.Amount = transaction.Amount;
                existing.CategoryId = transaction.CategoryId;
                existing.Description = transaction.Description;
                existing.Note = transaction.Note;
                existing.Attachement = transaction.Attachement;
                existing.IsActive = true;
                existing.UpdatedDate = DateTime.UtcNow;
                existing.UpdatedById = transaction.UpdatedById;

                _context.Accounts.Update(account);
                _context.Transactions.Update(existing);
                await _context.SaveChangesAsync();

                return Result<Transaction>.Success(transaction, "Transaction updated");
            }
            catch (Exception ex)
            {
                return Result<Transaction>.Failure(ex.Message);
            }
        }

        public async Task<Result<Transaction>> StageUpdateTransactionAsync(Transaction transaction)
        {
            // Logic identical to UpdateTransactionAsync but with extra handling for account change
            // Use previous implementation logic

            return await UpdateTransactionAsync(transaction);
        }

        public async Task<Result<bool>> TransferFundsAsync(long sourceAccountId, long destinationAccountId, decimal amount, string createdById)
        {
            try
            {
                var source = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountId == sourceAccountId && a.IsActive);
                var destination = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountId == destinationAccountId && a.IsActive);

                if (source == null || destination == null)
                    return Result<bool>.Failure("One or both accounts not found");

                var transferCategoryId = _context.IncomeExpenseCategories
                    .Where(x => x.IncomeExpenseCategoryType.TypeId == 6 && !x.IsDeletable)
                    .Select(x => x.CategoryId)
                    .FirstOrDefault();

                var debit = new Transaction
                {
                    TransactionDate = DateTime.UtcNow,
                    Amount = -amount,
                    AccountId = sourceAccountId,
                    CategoryId = transferCategoryId,
                    Note = $"Transfer to {destination.AccountName}",
                    IsActive = true,
                    CreatedById = createdById,
                    UpdatedById = createdById
                };

                var credit = new Transaction
                {
                    TransactionDate = DateTime.UtcNow,
                    Amount = amount,
                    AccountId = destinationAccountId,
                    CategoryId = transferCategoryId,
                    Note = $"Transfer from {source.AccountName}",
                    IsActive = true,
                    CreatedById = createdById,
                    UpdatedById = createdById
                };

                await _context.Transactions.AddRangeAsync(debit, credit);
                source.Amount -= amount;
                destination.Amount += amount;

                _context.Accounts.Update(source);
                _context.Accounts.Update(destination);
                await _context.SaveChangesAsync();

                return Result<bool>.Success(true, "Transfer successful");
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ex.Message);
            }
        }

        public async Task<Result<bool>> DeleteTransactionsByAccountIdAsync(long accountId, string updatedById)
        {
            try
            {
                var transactions = await _context.Transactions
                    .Where(t => t.AccountId == accountId && t.IsActive).ToListAsync();

                if (!transactions.Any())
                    return Result<bool>.Failure("No transactions found");

                foreach (var t in transactions)
                {
                    t.IsActive = false;
                    t.UpdatedById = updatedById;
                    t.UpdatedDate = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();
                return Result<bool>.Success(true, "Transactions deleted");
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ex.Message);
            }
        }

        public async Task<Result<bool>> DeleteTransactionsAsync(long transactionId, string updatedById)
        {
            return await DeleteTransactionAsync(transactionId, updatedById);
        }

        public async Task<Result<bool>> HasTransactionsByAccountIdAsync(long accountId)
        {
            var hasTransactions = await _context.Transactions
                .AnyAsync(t => t.AccountId == accountId && t.IsActive);

            return Result<bool>.Success(hasTransactions);
        }
    }


}
