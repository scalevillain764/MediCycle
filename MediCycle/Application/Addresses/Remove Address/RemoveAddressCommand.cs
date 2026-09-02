using MediatR;
using Infrastructure.Responding;
using Application.DTO.AddressDTO;
namespace Application.Addresses
{
    public record RemoveAddressCommand(Ulid addressId) : IRequest<Result<string>>;
}