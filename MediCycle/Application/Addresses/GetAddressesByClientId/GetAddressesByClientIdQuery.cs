using MediatR;
using Infrastructure.Responding;
using Application.DTO.AddressDTO;
namespace Application.Addresses
{
    public record GetAddressesByClientIdQuery(Ulid clientId) : IRequest<Result<List<AddressResponse>>;
}