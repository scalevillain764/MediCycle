using MediatR;
using Application.DTO.UserDTO;
using ICurrentUser = Application.Abstractions.ICurrentUser;
using Infrastructure.Responding;
using Infrastructure;
using Domain;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Text.Json;
namespace Application.Users 
{ 
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, Result<UserResponse>>
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUser _currentUser;
        private readonly IDatabase _redis;

        public GetUserQueryHandler(AppDbContext context, IConnectionMultiplexer redis, ICurrentUser currentUser)
        {
            _context = context;
            _currentUser = currentUser;
            _redis = redis.GetDatabase();
        }
        public async Task<Result<UserResponse>> Handle(GetUserQuery query, CancellationToken token)
        {
            if(_currentUser.UserId == query.userId)
            {
                var cachedUser = await _redis.StringGetAsync($"user:{_currentUser.UserId}");
                if(cachedUser.HasValue)
                {
                    var deserializedUser = JsonSerializer.Deserialize<UserResponse>((string)cachedUser!);

                    if (deserializedUser != null)
                        return Result<UserResponse>.Success(deserializedUser);
                }
            }

            var user = await _context.AllUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == query.userId, token);

            if (user == null) 
                return Result<UserResponse>.Error("Пользователь не найден", Domain.Enums.ErrorType.NotFound);

            UserResponse response = user switch
            {
                Worker w => new WorkerResponse(w.Id, w.Login, w.Name, w.Surname, w.Birthday),
                Client c => new ClientResponse(c.Id, c.Login, c.OrganizationName),
                _ => throw new InvalidOperationException($"Неизвестный тип пользователя: {user.GetType().Name}")
            };

            var serializedUser = JsonSerializer.Serialize((object)response); 

            await _redis.StringSetAsync($"user:{user.Id}", serializedUser, TimeSpan.FromHours(1));

            return Result<UserResponse>.Success(response);
        }
    }
}