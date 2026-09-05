using MediatR;
using ICurrentUser = Application.Abstractions.ICurrentUser;
using AddressResponse = Application.DTO.AddressDTO.AddressResponse;
using Address = Domain.Address;
using Infrastructure.Responding;
using Infrastructure;
using Error = Domain.Enums.ErrorType;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Text.Json;
namespace Application.Addresses
{
    public class GetAddressQueryHandler : IRequestHandler<GetAddressQuery, Result<AddressResponse>>
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUser _currentUser;
        private readonly IDatabase _redis;
        public GetAddressQueryHandler(AppDbContext context, ICurrentUser currentUser, IConnectionMultiplexer redis)
        {
            _context = context;
            _currentUser = currentUser;
            _redis = redis.GetDatabase();
        }
        public async Task<Result<AddressResponse>> Handle(GetAddressQuery query, CancellationToken token)
        {
            var cachedAddress = await _redis.StringGetAsync($"address:{query.addressId}");
            if(cachedAddress.HasValue)
            {
                var deserialized_address = JsonSerializer.Deserialize<AddressResponse>((string)cachedAddress!);
                if (deserialized_address != null)
                    return Result<AddressResponse>.Success(deserialized_address);
            }

            var address = await _context.Addresses
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == query.addressId, token);

            if (address == null)
                return Result<AddressResponse>.Error("Адрес не найден", Error.NotFound);

            if(_currentUser.UserType is "Client")
            {
                if (_currentUser.UserId != address.ClientId)
                    return Result<AddressResponse>.Error("Вы не можете увидеть этот адрес", Error.Forbidden);
            }

            var addressResponse = new AddressResponse(address);
            var serializedResponse = JsonSerializer.Serialize(addressResponse);

            await _redis.StringSetAsync($"address:{address.Id}", serializedResponse, TimeSpan.FromMinutes(10));  

            return Result<AddressResponse>.Success(addressResponse);
        }
    }
}