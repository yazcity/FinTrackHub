using FinTrackHub.Common;
using FinTrackHub.Entities;
using FinTrackHub.Identity;
using FinTrackHub.Interfaces;
using FinTrackHub.Models.DTOs;
using FinTrackHub.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinTrackHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : BaseController
    {
        private readonly IAccountRepository _accountRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITransactionRepository _transactionRepo;

        public AccountController(
            IAccountRepository accountRepo,
            UserManager<ApplicationUser> userManager,
            ITransactionRepository transactionRepo)
        {
            _accountRepo = accountRepo;
            _userManager = userManager;
            _transactionRepo = transactionRepo;
        }


        [HttpGet]
        public async Task<IActionResult> GetAccounts()
        {
            var accounts = await _accountRepo.GetAllAccountsAsync(null);
            return HandleResult(accounts);

        }

        [HttpPost]
        public async Task<IActionResult> AddEditAccount([FromBody] AccountDto model)
        {
            if (model == null || !ModelState.IsValid)
                return BadRequestResponse(GetModelErrors());


            var result = model.AccountId == 0
                ? await _accountRepo.AddAccountAsync(model)
                : await _accountRepo.UpdateAccountAsync(model);

            return HandleResult(result);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(long id)
        {
            try
            {
                if (id <= 0)
                    return BadRequestResponse("Invalid account ID.");

                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                    return UnauthorizedResponse("User not authenticated.");

                var txResult = await _transactionRepo.DeleteTransactionsByAccountIdAsync(id, user.Id);
                if (!txResult.IsSuccess)
                    return BadRequestResponse("Failed to delete related transactions.");

                var acctResult = await _accountRepo.DeleteAccountAsync(id, user.Id);
                return HandleResult(acctResult);
            }
            catch (Exception)
            {
                return ServerErrorResponse("Error deleting account. Try again later.");
            }
        }

        [HttpGet("user-accounts")]
        public async Task<IActionResult> GetAccountStats()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                    return UnauthorizedResponse("User not authenticated.");

                var accounts = await _accountRepo.GetAllAccountsAsync(user.Id);
                if (!accounts.IsSuccess)
                    return BadRequestResponse(accounts.Message);

                var totalAmount = accounts.Data.Sum(a => a.Amount);
                var totalPayable = accounts.Data.Sum(a => a.Payable);
                var balance = totalAmount;

                var result = new { TotalAmount = totalAmount, TotalPayable = totalPayable, Balance = balance };
                return OkResponse(result, "Account stats fetched.");
            }
            catch (Exception)
            {
                return ServerErrorResponse("Error fetching account statistics.");
            }
        }

        private string GetModelErrors()
        {
            return string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        }
    }

}
