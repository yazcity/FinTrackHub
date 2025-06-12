using FinTrackHub.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FinTrackHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected IActionResult OkResponse<T>(T data, string message) =>
           Ok(ApiResponse<T>.SuccessResponse(data, message, StatusCodes.Status200OK));

        protected IActionResult BadRequestResponse(string message) =>
            BadRequest(ApiResponse<string>.ErrorResponse(message, StatusCodes.Status400BadRequest));

        protected IActionResult UnauthorizedResponse(string message) =>
            Unauthorized(ApiResponse<string>.ErrorResponse(message, StatusCodes.Status401Unauthorized));

        protected IActionResult NotFoundResponse(string message) =>
            NotFound(ApiResponse<string>.ErrorResponse(message, StatusCodes.Status404NotFound));

        protected IActionResult ServerErrorResponse(string message) =>
            StatusCode(StatusCodes.Status500InternalServerError,
                ApiResponse<string>.ErrorResponse(message, StatusCodes.Status500InternalServerError));


        protected IActionResult HandleResult<T>(Result<T> result)
        {
            if (result.IsSuccess)
            {
                if (result.Data == null)
                    return NotFoundResponse("Requested resource not found.");

                return OkResponse(result.Data, result.Message);
            }

            if (result.Message.Contains("not found", StringComparison.OrdinalIgnoreCase))
                return NotFoundResponse(result.Message);

            if (result.Message.Contains("unauthorized", StringComparison.OrdinalIgnoreCase))
                return UnauthorizedResponse(result.Message);

            return ServerErrorResponse(result.Message);
        }
    }




}