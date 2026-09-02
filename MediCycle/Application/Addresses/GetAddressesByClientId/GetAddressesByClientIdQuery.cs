using MediatR;
using Infrastructure.Result;
using Application.DTO.AddressDTO;
namespace Application.Addresses
{
    public record GetAddressesByClientIdQuery(Ulid clientId) : IRequest<Result<List<AddressResponse>>;
}