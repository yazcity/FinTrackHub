using FinTrackHub.Data;
using FinTrackHub.Entities;
using FinTrackHub.Interfaces;
using FinTrackHub.Common;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using FinTrackHub.Services.Interfaces;
using FinTrackHub.Models.DTOs;
using AutoMapper;

namespace FinTrackHub.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public AccountRepository(ApplicationDbContext context, ITransactionRepository transactionRepository,
            ICurrentUserService currentUserService, IMapper mapper)
        {
            _context = context;
            _transactionRepository = transactionRepository;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<Result<AccountDto>> GetAccountByIdAsync(long accountId)
        {
            var UserId = _currentUserService.UserId;
            if (string.IsNullOrEmpty(UserId))
                return Result<AccountDto>.Failure("User is not authenticated");

            var account = await _context.Accounts
                                        .Include(a => a.AccountGroup)
                                            .ThenInclude(ag => ag.AccountGroupType)
                                        .Include(a => a.CreatedByUser)
                                        .Include(a => a.UpdatedByUser)
                                        .FirstOrDefaultAsync(a => a.AccountId == accountId);


            if (account == null)
                return Result<AccountDto>.Failure("Account not found");

            // 🟢 AutoMapper will handle the mapping
            var accountDto = _mapper.Map<AccountDto>(account);

            return Result<AccountDto>.Success(accountDto, "Account retrieved successfully");
        }

        public async Task<Result<IEnumerable<AccountDto>>> GetAllAccountsAsync(string? userId)
        {

            var LoggedUserId = _currentUserService.UserId;
            if (string.IsNullOrEmpty(LoggedUserId))
                return Result<IEnumerable<AccountDto>>.Failure("User is not authenticated");


            var currentUserId = userId ?? LoggedUserId;

            if (string.IsNullOrEmpty(currentUserId))
                return Result<IEnumerable<AccountDto>>.Failure("User is not authenticated");

            var accounts = await _context.Accounts
                                         .Where(x => x.IsActive && x.CreatedById == currentUserId)
                                         .Include(a => a.AccountGroup)
                                             .ThenInclude(ag => ag.AccountGroupType)
                                         .Include(a => a.CreatedByUser)
                                         .Include(a => a.UpdatedByUser)
                                         .ToListAsync();

            if (accounts == null || accounts.Count == 0)
                return Result<IEnumerable<AccountDto>>.Failure("No accounts found");

            try
            {
                var accountDtos = _mapper.Map<List<AccountDto>>(accounts);
                for (int i = 0; i < accountDtos.Count; i++)
                {
                    accountDtos[i].RowNumber = i + 1;
                }

                //    var accountDtos = _mapper.Map<IEnumerable<AccountDto>>(accounts);
                //                 //.Select((dto, index) => { dto.RowNumber = index + 1; return dto; })
                //                 //.ToList();

                return Result<IEnumerable<AccountDto>>.Success(accountDtos, "Accounts retrieved successfully");
            }
            catch (Exception ex)
            {
                // Log ex.ToString() here (e.g. ILogger), or return message for now
                return Result<IEnumerable<AccountDto>>.Failure($"Mapping error: {ex.Message}");
            }
        }


        public async Task<Result<AccountDto>> AddAccountAsync(AccountDto accountdto)
        {
            try
            {
                var UserId = _currentUserService.UserId;
                if (string.IsNullOrEmpty(UserId))
                    return Result<AccountDto>.Failure("User is not authenticated");


                var account = _mapper.Map<Account>(accountdto);
                account.CreatedById = UserId;
                account.UpdatedById = UserId;
                account.CreatedDate = DateTime.Now; 
                account.UpdatedDate = DateTime.Now;

                await _context.Accounts.AddAsync(account);
                await _context.SaveChangesAsync(); // AccountId is now generated

                var initialTransaction = new Transaction
                {
                    TransactionDate = DateTime.UtcNow,
                    Amount = account.Amount ?? 0,
                    AccountId = account.AccountId,
                    CategoryId = _context.IncomeExpenseCategories
                        .Where(x => x.IncomeExpenseCategoryType.TypeId == 1 && !x.IsDeletable)
                        .Select(z => z.CategoryId)
                        .FirstOrDefault(),
                    Note = "Initial deposit",
                    IsActive = true,
                    CreatedById = account.CreatedById,
                    UpdatedById = account.UpdatedById
                };

                var transactionResult = await _transactionRepository.AddTransactionAsync(initialTransaction);
                if (!transactionResult.IsSuccess)
                {
                    return Result<AccountDto>.Failure(transactionResult.Message);
                }

                // Reload account with related data for mapping
                var accountWithIncludes = await _context.Accounts
                    .Include(a => a.AccountGroup)
                        .ThenInclude(ag => ag.AccountGroupType)
                    .Include(a => a.CreatedByUser)
                    .Include(a => a.UpdatedByUser)
                    .FirstOrDefaultAsync(a => a.AccountId == account.AccountId);

                var accountDto = _mapper.Map<AccountDto>(accountWithIncludes);

                return Result<AccountDto>.Success(accountDto, "Account created successfully");
            }
            catch (Exception ex)
            {
                return Result<AccountDto>.Failure("An error occurred while creating the account: " + ex.Message);
            }
        }


        public async Task<Result<AccountDto>> UpdateAccountAsync(AccountDto account)
        {
            try
            {
                var UserId = _currentUserService.UserId;
                if (string.IsNullOrEmpty(UserId))
                    return Result<AccountDto>.Failure("User is not authenticated");


                var existingAccount = await _context.Accounts.FindAsync(account.AccountId);
                if (existingAccount == null)
                    return Result<AccountDto>.Failure("Account not found");

                if (existingAccount.Amount != account.Amount)
                {
                    var balanceChange = account.Amount - existingAccount.Amount;

                    var categoryId = _context.IncomeExpenseCategories
                        .Where(x => x.IncomeExpenseCategoryType.TypeId == 2 && !x.IsDeletable)
                        .Select(z => (long?)z.CategoryId)
                        .FirstOrDefault();

                    if (categoryId == null)
                        return Result<AccountDto>.Failure("Category for balance adjustment not found");

                    var balanceChangeTransaction = new Transaction
                    {
                        TransactionDate = DateTime.UtcNow,
                        Amount = balanceChange ?? 0,
                        AccountId = existingAccount.AccountId,
                        CategoryId = categoryId.Value,
                        Note = "Balance adjustment after account update",
                        IsActive = true,
                        CreatedById = UserId,
                        UpdatedById = UserId
                    };

                    var transactionResult = await _transactionRepository.AddTransactionAsync(balanceChangeTransaction);
                    if (!transactionResult.IsSuccess)
                    {
                        return Result<AccountDto>.Failure(transactionResult.Message);
                    }
                }

                // Update fields
                existingAccount.AccountName = account.AccountName;
                existingAccount.Amount = account.Amount;
                existingAccount.Payable = account.Payable;
                existingAccount.IsIncludeInTotal = account.IsIncludeInTotal;
                existingAccount.Description = account.Description;
                existingAccount.Note = account.Note;
                existingAccount.Attachement = account.Attachement;
                existingAccount.IsActive = true;
                existingAccount.UpdatedDate = DateTime.UtcNow;
                existingAccount.UpdatedById = UserId;
                existingAccount.AccountgroupId = account.AccountgroupId;

                await _context.SaveChangesAsync();

                var accountDto = _mapper.Map<AccountDto>(existingAccount);
                return Result<AccountDto>.Success(accountDto, "Account updated successfully");
            }
            catch (Exception ex)
            {
                return Result<AccountDto>.Failure("An error occurred while updating the account: " + ex.Message);
            }
        }


        public async Task<Result<string>> DeleteAccountAsync(long accountId, string updatedById)
        {
            try
            {
                var account = await _context.Accounts
                                            .SingleOrDefaultAsync(a => a.AccountId == accountId);

                if (account == null)
                    return Result<string>.Failure("Account not found");

                if (!account.IsActive)
                    return Result<string>.Failure("Account is already deleted");

                account.IsActive = false;
                account.UpdatedDate = DateTime.UtcNow;
                account.UpdatedById = updatedById;

                _context.Accounts.Update(account);
                await _context.SaveChangesAsync();

                return Result<string>.Success(account.AccountId.ToString(), "Account deleted successfully");
            }
            catch (Exception ex)
            {
                return Result<string>.Failure($"An error occurred while deleting the account: {ex.Message}");
            }
        }

    }
}
