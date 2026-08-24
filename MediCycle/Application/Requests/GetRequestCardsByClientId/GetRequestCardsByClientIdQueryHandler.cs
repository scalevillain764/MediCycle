using Application.DTO.RequestDTO;
using Infrastructure.Result;
using MediatR;
using Infrastructure;
using Error = Domain.Enums.ErrorType;
using Microsoft.EntityFrameworkCore;
namespace Application.Requests
{
    public class GetRequestCardsByClientIdQueryHandler : IRequestHandler<GetRequestCardsByClientIdQuery, Result<List<RequestResponseCard>>>
    {
        private readonly AppDbContext _context;
        public GetRequestCardsByClientIdQueryHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Result<List<RequestResponseCard>>> Handle(GetRequestCardsByClientIdQuery query, CancellationToken token)
        {
            if (!await _context.Clients.AnyAsync(x => x.Id == query.ClientId, token))
                return Result<List<RequestResponseCard>>.Error("Такого пользователя нет", Error.Conflict);

            var rez = await _context.Requests
                .Where(x => x.ClientId == query.ClientId)
                .Select(x => new RequestResponseCard(x))
                .AsNoTracking()
                .ToListAsync(token);

            return Result<List<RequestResponseCard>>.Success(rez);              
        }
    }
}