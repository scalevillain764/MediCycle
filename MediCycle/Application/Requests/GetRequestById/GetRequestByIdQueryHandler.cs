using Application.DTO.RequestDTO;
using Infrastructure.Result;
using MediatR;
using ICurrentUser = Application.Abstractions.ICurrentUser;
using Error = Domain.Enums.ErrorType;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Domain;
namespace Application.Requests
{
    public class GetRequestByIdQueryHandler : IRequestHandler<GetRequestByIdQuery, Result<RequestResponse>> // for admin
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUser _currentUser;
        public GetRequestByIdQueryHandler(AppDbContext context, ICurrentUser currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }
        public async Task<Result<RequestResponse>> Handle(GetRequestByIdQuery query, CancellationToken token)
        {
            var request = await _context.Requests
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == query.RequestId, token);

            if (request == null)
                return Result<RequestResponse>.Error("Заявка не найдена", Error.NotFound);

            if(_currentUser.UserType is "Client")
            {
                if (_currentUser.UserId != request.ClientId)
                    return Result<RequestResponse>.Error("Это не ваша заявка", Error.Forbidden);
            }

            if (_currentUser.UserRole is "Driver")
            {
                if (request.ExecutorId != _currentUser.UserId)
                    return Result<RequestResponse>.Error("Это не ваша заявка", Error.Forbidden);
            }

            return Result<RequestResponse>.Success(new RequestResponse(request));
        }
    }
}