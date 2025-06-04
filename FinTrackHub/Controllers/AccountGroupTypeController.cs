using FinTrackHub.Identity;
using FinTrackHub.Models;
using FinTrackHub.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinTrackHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountGroupTypeController : ControllerBase
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
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var groupTypes = await _service.GetGroupTypesAsync(user.Id);

            var result = groupTypes.Select(gt => new
            {
                gt.AccountgroupTypeId,
                gt.AccountGroupTypeName,
                gt.IsActive,
                gt.CreatedDate,
                gt.UpdatedDate,
                gt.IsDeletable
            });

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var groupType = await _service.GetByIdAsync(id);
            if (groupType == null)
                return NotFound();

            return Ok(groupType);
        }

        [HttpPost("add-edit")]
        public async Task<IActionResult> AddEdit([FromBody] AccountGroupTypeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(new { success = false, errors });
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            try
            {
                await _service.AddOrUpdateAsync(model, user.Id);
                return Ok(new { success = true, message = "Account group type saved successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            try
            {
                var success = await _service.DeleteAsync(id, user.Id);
                if (success)
                {
                    return Ok(new { success = true, message = "Account Group Type successfully deleted." });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Account Group Type is in use and cannot be deleted." });
                }
            }
            catch
            {
                return StatusCode(500, new { success = false, message = "An error occurred while processing your request." });
            }
        }
    }
}
