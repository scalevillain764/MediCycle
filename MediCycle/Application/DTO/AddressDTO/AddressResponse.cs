using Address = Domain.Address;
namespace Application.DTO.AddressDTO
{
    public record AddressResponse(Ulid id, Ulid clientId, string street, string buldingNumber, string presentativeName,
            string presentativeSurname, string presentativePhone)
    {
        public AddressResponse(Address address) : 
            this(
            address.Id,
            address.ClientId,
            address.Street,
            address.BuildingNumber,
            address.PresentativeName,
            address.PresentativeSurname,
            address.PresentativePhone) { }
    }
}