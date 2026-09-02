using Application.DTO.RequestDTO;
using Infrastructure.Responding;
using MediatR;
using Infrastructure;
using Error = Domain.Enums.ErrorType;
using ICurrentUser = Application.Abstractions.ICurrentUser;
using Microsoft.EntityFrameworkCore;
namespace Application.Requests
{
    public class GetRequestCardsByExecutorIdQueryHandler : IRequestHandler<GetRequestCardsByExecutorIdQuery, Result<PagedResponse<RequestResponseCard>>>
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUser _currentUser;
        public GetRequestCardsByExecutorIdQueryHandler(AppDbContext context, ICurrentUser currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }
        public async Task<Result<PagedResponse<RequestResponseCard>>> Handle(GetRequestCardsByExecutorIdQuery query, CancellationToken token)
        {
            if (_currentUser.UserRole is not "Dispatcher")
                return Result<PagedResponse<RequestResponseCard>>.Error("Вы не можете посмотреть заявки", Error.Forbidden);

            var baseQuery = _context.Requests
                .AsNoTracking()
                .Include(x => x.RequestAddress)
                .Include(x => x.Client)             
                .Where(x => x.ExecutorId == query.executorId);

            var totalCount = await baseQuery.CountAsync(token);

            var rez = await baseQuery       
               .Skip((query.page - 1) * query.pageSize)
               .Take(query.pageSize)
               .Select(x => new RequestResponseCard(x))            
               .ToListAsync(token);

            return Result<PagedResponse<RequestResponseCard>>.Success(new PagedResponse<RequestResponseCard>(rez, totalCount, query.page, query.pageSize));
        }
    }
}