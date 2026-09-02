using Application.DTO.RequestDTO;
using Infrastructure.Result;
using MediatR;
using Infrastructure;
using Error = Domain.Enums.ErrorType;
using ICurrentUser = Application.Abstractions.ICurrentUser;
using Microsoft.EntityFrameworkCore;
namespace Application.Requests
{
    public class GetRequestCardsByExecutorIdQueryHandler : IRequestHandler<GetRequestCardsByExecutorIdQuery, Result<List<RequestResponseCard>>>
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUser _currentUser;
        public GetRequestCardsByExecutorIdQueryHandler(AppDbContext context, ICurrentUser currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }
        public async Task<Result<List<RequestResponseCard>>> Handle(GetRequestCardsByExecutorIdQuery query, CancellationToken token)
        {
            if (_currentUser.UserRole is not "Dispatcher")
                return Result<List<RequestResponseCard>>.Error("Вы не можете посмотреть заявки", Error.Forbidden);

            var rez = await _context.Requests
               .Where(x => x.ExecutorId == query.executorId)
               .Include(x => x.RequestAddress)
               .Include(x => x.Client)
               .Select(x => new RequestResponseCard(x))
               .AsNoTracking()
               .ToListAsync(token);

            return Result<List<RequestResponseCard>>.Success(rez);
        }
    }
}