using Application.DTO.RequestDTO;
using Infrastructure;
using Infrastructure.Result;
using MediatR;
using Address = Domain.Address;
using AddressResponse = Application.DTO.AddressDTO.AddressResponse;
using Error = Domain.Enums.ErrorType;
using ICurrentUser = Application.Abstractions.ICurrentUser;
namespace Application.Addresses
{
    public class AddAddressCommandHandler : IRequestHandler<AddAddressCommand, Result<AddressResponse>>
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUser _currentUser;
        public AddAddressCommandHandler(AppDbContext context, ICurrentUser currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }
        public async Task<Result<AddressResponse>> Handle(AddAddressCommand command, CancellationToken token)
        {
            var company = await _context.Clients
                .FindAsync(_currentUser.UserId, token);
            
            if (company == null)
                return Result<AddressResponse>.Error("Организация не найдена", Error.Conflict);

            var address = new Address(company.Id, command.city, command.street, command.buldingNumber,
                command.presentativeName, command.presentativeSurname, command.presentativePhone);

            _context.Addresses.Add(address);

            await _context.SaveChangesAsync();

            return Result<AddressResponse>.Success(new AddressResponse(address));
        }
    }
}