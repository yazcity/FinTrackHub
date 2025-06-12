using FinTrackHub.Identity;
using FinTrackHub.Models.DTOs;
using FinTrackHub.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinTrackHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountGroupTypeController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAccountGroupTypeService _service;

        public AccountGroupTypeController(UserManager<ApplicationUser> userManager, IAccountGroupTypeService service)
        {
            _userManager = userManager;
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetGroupTypes()
        {
            try
            {
                var user = await GetCurrentUserAsync();
                if (user == null)
                    return UnauthorizedResponse("Unauthorized access.");

                var groupTypes = await _service.GetGroupTypesAsync(user.Id);

                if (groupTypes == null)
                    return NotFoundResponse("No account group types found.");

                if (!groupTypes.Any())
                    return OkResponse(groupTypes, "No account group types available.");

                return OkResponse(groupTypes, "Fetched successfully.");
            }
            catch (Exception)
            {
                return ServerErrorResponse("An unexpected error occurred while fetching group types.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                if (id <= 0)
                    return BadRequestResponse("Invalid account group type ID.");

                var groupType = await _service.GetByIdAsync(id);

                if (groupType == null)
                    return NotFoundResponse("Account group type not found.");

                return OkResponse(groupType, "Fetched successfully.");
            }
            catch (Exception)
            {
                return ServerErrorResponse("An unexpected error occurred while fetching the group type.");
            }
        }

        [HttpPost("add-edit")]
        public async Task<IActionResult> AddEdit([FromBody] AccountGroupTypeViewModel model)
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

                await _service.AddOrUpdateAsync(model, user.Id);
                return OkResponse<string>(null, "Account group type saved successfully.");
            }
            catch (Exception)
            {
                return ServerErrorResponse("An unexpected error occurred while saving the account group type.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                if (id <= 0)
                    return BadRequestResponse("Invalid account group type ID.");

                var user = await GetCurrentUserAsync();
                if (user == null)
                    return UnauthorizedResponse("Unauthorized access.");

                var success = await _service.DeleteAsync(id, user.Id);

                if (!success)
                    return BadRequestResponse("Account Group Type is in use and cannot be deleted.");

                return OkResponse<string>(null, "Account Group Type successfully deleted.");
            }
            catch (Exception)
            {
                return ServerErrorResponse("An unexpected error occurred while deleting the account group type.");
            }
        }

        private string GetModelErrors()
        {
            return string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        }

        private async Task<ApplicationUser?> GetCurrentUserAsync()
        {
            return await _userManager.GetUserAsync(User);
        }
    }

}
