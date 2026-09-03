using MediatR;
using Infrastructure.Responding;
using Application.DTO.AddressDTO;
namespace Application.Addresses
{
    public record EditAddressCommand(string city, string street, string buldingNumber, string presentativeName,
            string presentativeSurname, string presentativePhone, Ulid addressId = default) : IRequest<Result<AddressResponse>>;
}