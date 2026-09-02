using MediatR;
using ICurrentUser = Application.Abstractions.ICurrentUser;
using AddressResponse = Application.DTO.AddressDTO.AddressResponse;
using Address = Domain.Address;
using Infrastructure.Responding;
using Infrastructure;
using Error = Domain.Enums.ErrorType;
namespace Application.Addresses
{
    public class EditAddressCommandHandler : IRequestHandler<EditAddressCommand, Result<AddressResponse>>
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUser _currentUser;
        public EditAddressCommandHandler(AppDbContext context, ICurrentUser currentUser) { 
            _context = context;
            _currentUser = currentUser;
        }
        public async Task<Result<AddressResponse>> Handle(EditAddressCommand command, CancellationToken token)
        {
            var address = await _context.Addresses
                .FindAsync(command.addressId, token);

            if (address == null)
                return Result<AddressResponse>.Error("Адрес не найден", Error.NotFound);

            if (address.ClientId != _currentUser.UserId)
                return Result<AddressResponse>.Error("Это не ваш адрес", Error.Forbidden);

            address.City = command.city;
            address.Street = command.street;
            address.BuildingNumber = command.buldingNumber;
            address.PresentativeName = command.presentativeName;
            address.PresentativeSurname = command.presentativeSurname;
            address.PresentativePhone = command.presentativePhone;

            await _context.SaveChangesAsync(token);

            return Result<AddressResponse>.Success(new AddressResponse(address)); 
        } 
    }
}