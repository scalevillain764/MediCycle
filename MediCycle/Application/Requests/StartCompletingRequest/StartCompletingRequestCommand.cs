using Application.DTO.RequestDTO;
using Infrastructure;
using Infrastructure.Result;
using MediatR;
using Request = Domain.Request;
using Error = Domain.Enums.ErrorType;
using RequestStatus = Domain.Enums.RequestStatus;
using Microsoft.EntityFrameworkCore;
using ICurrentUser = Application.Abstractions.ICurrentUser;
namespace Application.Requests
{
    public class StartCompletingRequestCommandHandler : IRequestHandler<StartCompletingRequestCommand, Result<RequestResponse>>
    {
        private readonly ICurrentUser _currentUser;
        private readonly AppDbContext _context;
        public StartCompletingRequestCommandHandler(ICurrentUser currentUser, AppDbContext context)
        {
            _currentUser = currentUser;
            _context = context;
        }
        public async Task<Result<RequestResponse>> Handle(StartCompletingRequestCommand command, CancellationToken token)
        {
            var request = await _context.Requests
                .Include(x => x.RequestAddress)
                .Include(x => x.Client)
                .Include(x => x.Executor)
                .FirstOrDefaultAsync(x => x.Id == command.requestId, token);

            if (request == null)
                return Result<RequestResponse>.Error("Заявка не найдена", Error.NotFound);

            if (request.Status != RequestStatus.Assigned)
                return Result<RequestResponse>.Error("Эту заявку нельзя начать выполнять", Error.Forbidden);

            if (_currentUser.UserRole is not "Driver")
                return Result<RequestResponse>.Error("Заявки могут выполнять только водители", Error.Conflict);

            if (request.ExecutorId != _currentUser.UserId)
                return Result<RequestResponse>.Error("Вы не можете начать выполнять эту заявку", Error.Forbidden);

            request.Status = RequestStatus.InProgress;

            await _context.SaveChangesAsync(token);

            return Result<RequestResponse>.Success(new RequestResponse(request));
        }
    }
}