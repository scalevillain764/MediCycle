using MediatR;
using Infrastructure.Result;
using Application.DTO.AddressDTO;
namespace Application.Addresses
{
    public record RemoveAddressCommand(Ulid addressId) : IRequest<Result<string>>;
}