using MediatR;
using Infrastructure.Result;
using Application.DTO.AddressDTO;
namespace Application.Addresses
{
    public record AddAddressCommand(string city, string street, string buldingNumber, string presentativeName,
            string presentativeSurname, string presentativePhone)
        : IRequest<Result<AddressResponse>>;
}