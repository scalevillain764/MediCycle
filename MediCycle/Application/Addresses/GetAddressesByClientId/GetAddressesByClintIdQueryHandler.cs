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
    public class GetAddressesByClientIdQueryHandler : IRequestHandler<GetAddressesByClientIdQuery, Result<List<AddressResponse>>>
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUser _currentUser;
        private readonly IDatabase _redis;
        public GetAddressesByClientIdQueryHandler(AppDbContext context, ICurrentUser currentUser, IConnectionMultiplexer redis)
        {
            _context = context;
            _currentUser = currentUser;
            _redis = redis.GetDatabase();
        }
        public async Task<Result<List<AddressResponse>>> Handle(GetAddressesByClientIdQuery query, CancellationToken token)
        {
            Ulid? clientId = _currentUser.UserType is "Client" ? _currentUser.UserId : query.clientId;

            if (clientId == null)
                return Result<List<AddressResponse>>.Error("Ошибка авторизации", Error.Unauthorized);

            var cache = await _redis.StringGetAsync($"user:addresses:{clientId}");

            if(cache.HasValue)
            {
                var deserializedList = JsonSerializer.Deserialize<List<AddressResponse>>((string)cache!);

                if (deserializedList != null)
                {
                    return Result<List<AddressResponse>>.Success(deserializedList);
                }
            }

            var rez = await _context.Addresses
                .Where(x => x.ClientId == clientId)
                .Select(x => new AddressResponse(x))
                .AsNoTracking()
                .ToListAsync(token);

            if(rez.Count > 0)
            {
                var serializedList = JsonSerializer.Serialize(rez);
                await _redis.StringSetAsync($"user:addresses:{clientId}", serializedList, TimeSpan.FromHours(3));
            }

            return Result<List<AddressResponse>>.Success(rez);
        }
    }
}