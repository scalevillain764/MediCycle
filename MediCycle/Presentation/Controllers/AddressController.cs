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
    [Authorize(Policy = "ClientOnly")]
    public class AddressController : BaseController
    {
        private readonly IMediator _mediator;
        public AddressController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> AddAddressAsync([FromBody] AddAddressCommand command, CancellationToken token)
            => ProcessResult(await _mediator.Send(command, token));

        [HttpPatch("{addressId}/edit")]
        public async Task<IActionResult> EditAddressAsync([FromQuery] Ulid addressId, [FromBody] EditAddressCommand command, CancellationToken token)
            => ProcessResult(await _mediator.Send(command with { addressId = addressId}, token));
    }
}