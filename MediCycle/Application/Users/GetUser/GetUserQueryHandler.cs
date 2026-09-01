using MediatR;
using Application.DTO.UserDTO;
using ICurrentUser = Application.Abstractions.ICurrentUser;
using Infrastructure.Result;
using Infrastructure;
using Domain;
using Microsoft.EntityFrameworkCore;
namespace Application.Users 
{ 
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, Result<UserResponse>>
    {
        private readonly AppDbContext _context;
        public GetUserQueryHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Result<UserResponse>> Handle(GetUserQuery query, CancellationToken token)
        {
            var user = await _context.AllUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == query.userId, token);

            if (user == null)
                return Result<UserResponse>.Error("Пользователь не найден", Domain.Enums.ErrorType.NotFound);

            return Result<UserResponse>.Success(user switch
            {
                  Worker w => new WorkerResponse(w.Id, w.Login, w.Name, w.Surname, w.Birthday),
                  Client c => new ClientResponse(c.Id, c.Login, c.OrganizationName),
                  _ => throw new InvalidOperationException($"Неизвестный тип пользователя: {user.GetType().Name}")
            });
        }
    }
}