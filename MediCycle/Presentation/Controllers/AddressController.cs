using MediatR;
using Application.DTO.AddressDTO;
using Address = Domain.Address;
using Microsoft.AspNetCore.Mvc;
using Application.Addresses;
using Microsoft.AspNetCore.Authorization;
namespace Presentation.Controllers
{
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [Authorize]
    public class AddressController : BaseController
    {
        private readonly IMediator _mediator;
        public AddressController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Policy = "ClientOnly")]
        public async Task<IActionResult> AddAddressAsync([FromBody] AddAddressCommand command, CancellationToken token)
            => ProcessResult(await _mediator.Send(command, token));

        [HttpPatch("{addressId}/edit")]
        [Authorize(Policy = "ClientOnly")]
        public async Task<IActionResult> EditAddressAsync([FromQuery] Ulid addressId, [FromBody] EditAddressCommand command, CancellationToken token)
            => ProcessResult(await _mediator.Send(command with { addressId = addressId}, token));

        [HttpGet("{addressId}")]
        public async Task<IActionResult> GetAdddressByIdAsync([FromQuery] Ulid addressId, CancellationToken token)
            => ProcessResult(await _mediator.Send(new GetAddressQuery(addressId), token));

        [HttpGet("client/{clientId}")]
        public async Task<IActionResult> GetAddressesByClientIdAsync([FromQuery] Ulid clientId, CancellationToken token) 
            => ProcessResult(await _mediator.Send(new GetAddressesByClientIdQuery(clientId), token));

        [HttpGet("my")]
        public async Task<IActionResult> GetMyAddressesAsync(CancellationToken token)
            => ProcessResult(await _mediator.Send(new GetAddressesByClientIdQuery(CurrentUserId), token));

        [HttpDelete("{addressId}")]
        [Authorize(Policy = "ClientOnly")]
        public async Task<IActionResult> RemoveAddressAsync([FromQuery] Ulid addressId, CancellationToken token)
            => ProcessResult(await _mediator.Send(new RemoveAddressCommand(addressId), token));
    }
}