using FinTrackHub.Identity;
using FinTrackHub.Interfaces;
using FinTrackHub.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinTrackHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountGroupController : BaseController
    {
        private readonly IAccountGroupRepository _accountGroupRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountGroupController(IAccountGroupRepository accountGroupRepository, UserManager<ApplicationUser> userManager)
        {
            _accountGroupRepository = accountGroupRepository;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetGroups()
        {
            try
            {
                var user = await GetCurrentUserAsync();
                if (user == null) return UnauthorizedResponse("Unauthorized access.");

                var groups = await _accountGroupRepository.GetGroupsByUserIdAsync(user.Id);

                if (groups == null)
                    return NotFoundResponse("No account groups found.");
                if (!groups.Any())
                    return OkResponse(groups, "No groups available for this user.");

                return OkResponse(groups, "Groups fetched successfully.");
            }
            catch (Exception ex)
            {
                return ServerErrorResponse("An unexpected error occurred while fetching groups.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                if (id <= 0)
                    return BadRequestResponse("Invalid group ID.");

                var group = await _accountGroupRepository.GetByIdAsync(id);
                if (group == null)
                    return NotFoundResponse("Account group not found.");

                return OkResponse(group, "Group fetched successfully.");
            }
            catch (Exception ex)
            {
                return ServerErrorResponse("An unexpected error occurred while fetching the group.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit([FromBody] AccountGroupViewModel model)
        {
            try
            {
                if (model == null)
                    return BadRequestResponse("Request payload is missing.");
                if (!ModelState.IsValid)
                    return BadRequestResponse(GetModelErrors());

                var user = await GetCurrentUserAsync();
                if (user == null)
                    return UnauthorizedResponse("Unauthorized access.");

                var result = await _accountGroupRepository.AddOrUpdateAsync(model, user.Id);
                if (!result.Success)
                    return BadRequestResponse(result.Message);

                return OkResponse(result, "Account group saved successfully.");
            }
            catch (Exception ex)
            {
                return ServerErrorResponse("An unexpected error occurred while saving the account group.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                if (id <= 0)
                    return BadRequestResponse("Invalid group ID.");

                var user = await GetCurrentUserAsync();
                if (user == null)
                    return UnauthorizedResponse("Unauthorized access.");

                var result = await _accountGroupRepository.DeleteAsync(id, user.Id);
                if (!result.Success)
                    return BadRequestResponse(result.Message);

                return OkResponse(result, "Account group deleted successfully.");
            }
            catch (Exception ex)
            {
                return ServerErrorResponse("An unexpected error occurred while deleting the account group.");
            }
        }

        // Helper: Get logged-in user
        private async Task<ApplicationUser?> GetCurrentUserAsync()
        {
            return await _userManager.GetUserAsync(User);
        }

        // Helper: Aggregate model validation errors
        private string GetModelErrors()
        {
            return string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        }
    }

}
