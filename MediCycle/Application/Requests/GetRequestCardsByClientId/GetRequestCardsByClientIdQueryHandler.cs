using Application.DTO.RequestDTO;
using Infrastructure.Result;
using MediatR;
using Infrastructure;
using Error = Domain.Enums.ErrorType;
using ICurrentUser = Application.Abstractions.ICurrentUser;
using Microsoft.EntityFrameworkCore;
namespace Application.Requests
{
    public class GetRequestCardsByClientIdQueryHandler : IRequestHandler<GetRequestCardsByClientIdQuery, Result<List<RequestResponseCard>>>
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUser _currentUser;
        public GetRequestCardsByClientIdQueryHandler(AppDbContext context, ICurrentUser currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }
        public async Task<Result<List<RequestResponseCard>>> Handle(GetRequestCardsByClientIdQuery query, CancellationToken token)
        {
            if(_currentUser.UserType is "Client")
            {
                if (_currentUser.UserId != query.ClientId)
                    return Result<List<RequestResponseCard>>.Error("Вы не можете посмотреть заявки", Error.Forbidden);
            }

            if (_currentUser.UserRole is "Driver")
                return Result<List<RequestResponseCard>>.Error("Вы не можете посмотреть заявки", Error.Forbidden);
 
             var rez = await _context.Requests
                .Where(x => x.ClientId == query.ClientId)
                .Select(x => new RequestResponseCard(x))
                .AsNoTracking()
                .ToListAsync(token);

            return Result<List<RequestResponseCard>>.Success(rez);              
        }
    }
}