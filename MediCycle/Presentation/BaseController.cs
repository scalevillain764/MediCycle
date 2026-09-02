using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Infrastructure.Responding;
using Error = Domain.Enums.ErrorType;
namespace Presentation
{
    [ApiController]
    public class BaseController : ControllerBase {
        protected Ulid CurrentUserId => ExtractUserIdFromClaims();
        private Ulid ExtractUserIdFromClaims()
            => Ulid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId)
            ? userId : throw new UnauthorizedAccessException();
        protected IActionResult ProcessResult<T>(Result<T> data) where T : class
        {
            if(!data.IsSuccess)
            {
                return data.ErrorType switch
                {
                    Error.Conflict => Conflict(data.ErrorMessage),
                    Error.NotFound => NotFound(data.ErrorMessage),
                    Error.Validation => BadRequest(data.ErrorMessage),
                    Error.Forbidden => Forbid(data.ErrorMessage),
                    Error.Unauthorized => Unauthorized(data.ErrorMessage)
                };
            }

            return Ok(data.Content);
        }
    }
}