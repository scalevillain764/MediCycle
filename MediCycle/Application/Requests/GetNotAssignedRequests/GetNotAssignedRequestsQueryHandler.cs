using Application.DTO.RequestDTO;
using Infrastructure;
using Infrastructure.Responding;
using MediatR;
using ICurrentUser = Application.Abstractions.ICurrentUser;
using Error = Domain.Enums.ErrorType;
using Microsoft.EntityFrameworkCore;
namespace Application.Requests
{
    public class GetNotAssignedRequestsQueryHandler : IRequestHandler<GetNotAssignedRequestsQuery, Result<PagedResponse<RequestResponseCard>>>
    {
        private readonly AppDbContext _context;
        public GetNotAssignedRequestsQueryHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Result<PagedResponse<RequestResponseCard>>> Handle(GetNotAssignedRequestsQuery query, CancellationToken token)
        {
            var rez = await _context.Requests
                .Where(x => x.Status.ToString() == query.status)
                .Skip((query.page - 1) * query.pageSize)
                .Take(query.pageSize)
                .Select(x => new RequestResponseCard(x))
                .ToListAsync(token);

            return Result<PagedResponse<RequestResponseCard>>.Success(new PagedResponse<RequestResponseCard>(rez, query.page, query.pageSize));
        }
    }
}