using Domain;
using Infrastructure;
using Infrastructure.Responding;
using MediatR;
using StackExchange.Redis;
using Address = Domain.Address;
using AddressResponse = Application.DTO.AddressDTO.AddressResponse;
using Error = Domain.Enums.ErrorType;
using ICurrentUser = Application.Abstractions.ICurrentUser;
namespace Application.Addresses
{
    public class EditAddressCommandHandler : IRequestHandler<EditAddressCommand, Result<AddressResponse>>
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUser _currentUser;
        private readonly IDatabase _redis;
        public EditAddressCommandHandler(AppDbContext context, ICurrentUser currentUser, IConnectionMultiplexer redis) { 
            _context = context;
            _currentUser = currentUser;
            _redis = redis.GetDatabase();
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
            await _redis.KeyDeleteAsync($"address:{address.Id}");

            return Result<AddressResponse>.Success(new AddressResponse(address)); 
        } 
    }
}