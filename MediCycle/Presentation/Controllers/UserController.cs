using MediatR;
using Application.DTO.UserDTO;
using User = Domain.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Application.Users;
namespace Presentation.Controllers
{
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [Authorize]
    public class UserController : BaseController
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMeAsync(CancellationToken token)
            => ProcessResult(await _mediator.Send(new GetUserQuery(CurrentUserId), token));

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserAsync([FromRoute] Ulid userId, CancellationToken token)
            => ProcessResult(await _mediator.Send(new GetUserQuery(userId), token));


        [HttpGet("me/edit")]
        public async Task<IActionResult> EditMeAsync([FromBody] EditUserCommand command, CancellationToken token)
            => ProcessResult(await _mediator.Send(command, token));
    }
}