using FinTrackHub.Common;
using FinTrackHub.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FinTrackHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DropdownController : BaseController
    {
        private readonly IDropdownRepository _dropdownRepository;

        public DropdownController(IDropdownRepository dropdownRepository)
        {
            _dropdownRepository = dropdownRepository;
        }

        [HttpGet("account-group-types")]
        [ProducesResponseType(typeof(ApiResponse<List<SelectListItem>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAccountGroupTypes()
        {
            var result = await _dropdownRepository.GetAccountGroupTypesAsync();
            return HandleResult(result);
        }

        [HttpGet("account-groups/{typeId:long}")]
        [ProducesResponseType(typeof(ApiResponse<List<SelectListItem>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAccountGroups(long typeId)
        {
            var result = await _dropdownRepository.GetAccountGroupsAsync(typeId);
            return HandleResult(result);
        }

        [HttpGet("income-expense-types")]
        [ProducesResponseType(typeof(ApiResponse<List<SelectListItem>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetIncomeExpenseTypes()
        {
            var result = await _dropdownRepository.GetIncomeExpenseCategoryTypesAsync();
            return HandleResult(result);
        }

        [HttpGet("income-expense-categories/{typeId:long}")]
        [ProducesResponseType(typeof(ApiResponse<List<SelectListItem>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetIncomeExpenseCategories(long typeId)
        {
            var result = await _dropdownRepository.GetIncomeExpenseCategoriesAsync(typeId);
            return HandleResult(result);
        }

        [HttpGet("accounts")]
        [ProducesResponseType(typeof(ApiResponse<List<SelectListItem>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAccounts()
        {
            var result = await _dropdownRepository.GetAccountsAsync();
            return HandleResult(result);
        }
    }
}
