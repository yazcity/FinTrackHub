using FinTrackHub.Common;
using FinTrackHub.Interfaces;
using FinTrackHub.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FinTrackHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionCategoryController : BaseController
    {
        private readonly ITransactionCategoryRepository _repository;

        public TransactionCategoryController(ITransactionCategoryRepository repository)
        {
            _repository = repository;
        }

        // GET: api/TransactionCategory
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _repository.GetAllCategoriesAsync();
            return HandleResult(result);
        }

        // GET: api/TransactionCategory/5
        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _repository.GetCategoryByIdAsync(id);
            return HandleResult(result);
        }

        // POST: api/TransactionCategory
        [HttpPost]
        public async Task<IActionResult> Save([FromBody] TransactionCategoryDto model)
        {
            if (!ModelState.IsValid)
                return BadRequestResponse("Invalid model data.");

            var result = await _repository.SaveCategoryAsync(model);
            return HandleResult(result);
        }

        // DELETE: api/TransactionCategory/5
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _repository.DeleteCategoryAsync(id);
            return HandleResult(result);
        }
    }

}
