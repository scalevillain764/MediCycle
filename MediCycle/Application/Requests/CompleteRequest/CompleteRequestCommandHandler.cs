using Application.DTO.RequestDTO;
using Infrastructure;
using Infrastructure.Responding;
using MediatR;
using Request = Domain.Request;
using Error = Domain.Enums.ErrorType;
using RequestStatus = Domain.Enums.RequestStatus;
using Microsoft.EntityFrameworkCore;
using ICurrentUser = Application.Abstractions.ICurrentUser;
namespace Application.Requests
{
    public class CompleteRequestCommandHandler : IRequestHandler<CompleteRequestCommand, Result<RequestResponse>>
    {
        private readonly ICurrentUser _currentUser;
        private readonly AppDbContext _context;
        public CompleteRequestCommandHandler(ICurrentUser currentUser, AppDbContext context)
        {
            _currentUser = currentUser;
            _context = context;
        }
        public async Task<Result<RequestResponse>> Handle(CompleteRequestCommand command, CancellationToken token)
        {
            var request = await _context.Requests
                .Include(x => x.RequestAddress)
                .Include(x => x.Client)
                .Include(x => x.Executor)
                .FirstOrDefaultAsync(x => x.Id == command.requestId, token);

            if (request == null)
                return Result<RequestResponse>.Error("Заявка не найдена", Error.NotFound);

            if (request.Status != RequestStatus.InProgress)
                return Result<RequestResponse>.Error("Эту заявку нельзя завершить", Error.Forbidden);

            if (_currentUser.UserRole is not "Driver")
                return Result<RequestResponse>.Error("Заявки могут завершать только водители", Error.Conflict);

            if (request.ExecutorId != _currentUser.UserId)
                return Result<RequestResponse>.Error("Вы не можете завершить эту заявку", Error.Forbidden);

            request.Status = RequestStatus.Completed;
            await _context.SaveChangesAsync(token);

            return Result<RequestResponse>.Success(new RequestResponse(request));
        }
    }
}