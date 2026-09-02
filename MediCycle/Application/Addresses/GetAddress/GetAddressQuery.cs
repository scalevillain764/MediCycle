using MediatR;
using Infrastructure.Responding;
using Application.DTO.AddressDTO;
namespace Application.Addresses
{
    public record GetAddressQuery(Ulid addressId): IRequest<Result<AddressResponse>>;
}