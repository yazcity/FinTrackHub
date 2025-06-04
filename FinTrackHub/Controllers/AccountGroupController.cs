using FinTrackHub.Identity;
using FinTrackHub.Interfaces;
using FinTrackHub.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinTrackHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountGroupController : ControllerBase
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
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var groups = await _accountGroupRepository.GetGroupsByUserIdAsync(user.Id);
            return Ok(groups);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var group = await _accountGroupRepository.GetByIdAsync(id);
            if (group == null) return NotFound();

            return Ok(group);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit([FromBody] AccountGroupViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var result = await _accountGroupRepository.AddOrUpdateAsync(model, user.Id);
            if (!result.Success) return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var result = await _accountGroupRepository.DeleteAsync(id, user.Id);
            if (!result.Success) return BadRequest(result.Message);

            return Ok(result);
        }

    }
}
