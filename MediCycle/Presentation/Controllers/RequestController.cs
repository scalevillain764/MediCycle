using Application.DTO.RequestDTO;
using Application.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
namespace Presentation.Controllers 
{
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [Authorize]
    public class RequestController : BaseController
    {
        private readonly IMediator _mediator;
        public RequestController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Policy = "ClientOnly")]
        public async Task<IActionResult> AddRequestAsync([FromBody] AddRequestCommand command, CancellationToken token)
            => ProcessResult(await _mediator.Send(command, token));

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> RemoveRequestAsync([FromBody] RemoveRequestCommand command, CancellationToken token)
            => ProcessResult(await _mediator.Send(command, token));

        [HttpPatch("{requestId}/cancel")]
        [Authorize(Policy = "ClientOnly")]
        public async Task<IActionResult> CancelRequestAsync([FromRoute] Ulid requestId, CancellationToken token)
            => ProcessResult(await _mediator.Send(new CancelRequestCommand(requestId), token));

        [HttpPatch("{requestId}/complete")]
        [Authorize(Roles = "Driver")]
        public async Task<IActionResult> CompleteRequestAsync([FromRoute] Ulid requestId, CancellationToken token)
            => ProcessResult(await _mediator.Send(new CompleteRequestCommand(requestId), token));

        [HttpPatch("{requestId}/assign_to/{ExecutorId}")]
        [Authorize(Roles = "Dispatcher")]
        public async Task<IActionResult> AssignRequestAsync([FromRoute] Ulid requestId, [FromRoute] Ulid ExecutorId, CancellationToken token)
            => ProcessResult(await _mediator.Send(new AssignRequestCommand(requestId, ExecutorId), token));

        [HttpPatch("{requestId}/start")]
        [Authorize(Roles = "Driver")]
        public async Task<IActionResult> StartCompletingRequestAsync([FromRoute] Ulid requestId, CancellationToken token)
            => ProcessResult(await _mediator.Send(new StartCompletingRequestCommand(requestId), token));

        [HttpPatch("{requestId}/edit")]
        [Authorize(Policy = "ClientOnly")]
        public async Task<IActionResult> EditRequestAsync([FromRoute] Ulid requestId, [FromBody] EditRequestCommand command, CancellationToken token)
            => ProcessResult(await _mediator.Send(command with {RequestId = requestId}, token));

        // add get methods
    }
}
