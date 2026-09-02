using Application.DTO.RequestDTO;
using MediatR;
using Infrastructure.Responding;
using RequestStatus = Domain.Enums.RequestStatus;
using Error = Domain.Enums.ErrorType;
using Infrastructure;
using ICurrentUser = Application.Abstractions.ICurrentUser;
using Microsoft.EntityFrameworkCore;
namespace Application.Requests
{
    public class CancelRequestCommandHandler : IRequestHandler<CancelRequestCommand, Result<RequestResponse>>
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUser _currentUser;
        public CancelRequestCommandHandler(AppDbContext context, ICurrentUser currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }
        public async Task<Result<RequestResponse>> Handle(CancelRequestCommand command, CancellationToken token)
        {           
            var request = await _context.Requests
                .FindAsync(command.RequestId, token);

            if (request == null)
                return Result<RequestResponse>.Error("Заявка не найдена", Error.NotFound);

            if(_currentUser.UserId != request.ClientId)
                return Result<RequestResponse>.Error("Вы не можете отменить заявку", Error.Forbidden);

            request.Status = RequestStatus.Cancelled;

            await _context.SaveChangesAsync(token);

            return Result<RequestResponse>.Success(new RequestResponse(request));
        }
    }
}