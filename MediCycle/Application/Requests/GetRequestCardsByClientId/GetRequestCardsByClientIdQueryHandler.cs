using Application.DTO.RequestDTO;
using Infrastructure.Responding;
using MediatR;
using Infrastructure;
using Error = Domain.Enums.ErrorType;
using ICurrentUser = Application.Abstractions.ICurrentUser;
using Microsoft.EntityFrameworkCore;
namespace Application.Requests
{
    public class GetRequestCardsByClientIdQueryHandler : IRequestHandler<GetRequestCardsByClientIdQuery, Result<PagedResponse<RequestResponseCard>>>
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUser _currentUser;
        public GetRequestCardsByClientIdQueryHandler(AppDbContext context, ICurrentUser currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }
        public async Task<Result<PagedResponse<RequestResponseCard>>> Handle(GetRequestCardsByClientIdQuery query, CancellationToken token)
        {
            if(_currentUser.UserType is "Client")
            {
                if (_currentUser.UserId != query.ClientId)
                    return Result<PagedResponse<RequestResponseCard>>.Error("Вы не можете посмотреть заявки", Error.Forbidden);
            }

            if (_currentUser.UserRole is "Driver")
                return Result<PagedResponse<RequestResponseCard>>.Error("Вы не можете посмотреть заявки", Error.Forbidden);

            var baseQuery = _context.Requests
                .AsNoTracking()
                .Include(x => x.RequestAddress)
                .Include(x => x.Client)
                .Where(x => x.ClientId == query.ClientId);

            var totalCount = await baseQuery.CountAsync(token);

            var rez = await baseQuery
               .Skip((query.Page - 1) * query.PageSize)
               .Take(query.PageSize)
               .Select(x => new RequestResponseCard(x))
               .ToListAsync(token);

            return Result<PagedResponse<RequestResponseCard>>.Success(new PagedResponse<RequestResponseCard>(rez, totalCount, query.Page, query.PageSize));              
        }
    }
}