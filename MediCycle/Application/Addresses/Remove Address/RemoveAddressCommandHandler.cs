using Domain;
using Infrastructure;
using Infrastructure.Responding;
using MediatR;
using Address = Domain.Address;
using AddressResponse = Application.DTO.AddressDTO.AddressResponse;
using Error = Domain.Enums.ErrorType;
using ICurrentUser = Application.Abstractions.ICurrentUser;
using StackExchange.Redis;
namespace Application.Addresses
{
    public class RemoveAddressCommandHandler : IRequestHandler<RemoveAddressCommand, Result<string>>
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUser _currentUser;
        private readonly IDatabase _redis;
        public RemoveAddressCommandHandler(AppDbContext context, ICurrentUser currentUser, IConnectionMultiplexer redis)
        {
            _context = context;
            _currentUser = currentUser;
            _redis = redis.GetDatabase();
        }
        public async Task<Result<string>> Handle(RemoveAddressCommand command, CancellationToken token)
        {
            var address = await _context.Addresses
                .FindAsync(command.addressId, token);

            if (address == null)
                return Result<string>.Error("Адрес не найден", Error.NotFound);

            if (address.ClientId != _currentUser.UserId)
                return Result<string>.Error("Это не ваш адрес", Error.Forbidden);

            _context.Addresses.Remove(address);

            await _context.SaveChangesAsync(token);
            await _redis.KeyDeleteAsync($"address:{command.addressId}");

            return Result<string>.Success("OK");
        }
    }
}