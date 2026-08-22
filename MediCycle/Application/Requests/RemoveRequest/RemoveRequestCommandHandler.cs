using MediatR;
using ICurrentUser = Application.Abstractions.ICurrentUser;
using Infrastructure.Result;
using Infrastructure;
using Error = Domain.Enums.ErrorType;
namespace Application.Requests.RemoveRequest
{
    public class RemoveRequestCommandHandler : IRequestHandler<RemoveRequestCommand, Result<string>>
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUser _currentUser;
        public RemoveRequestCommandHandler(AppDbContext context, ICurrentUser currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }
        public async Task<Result<string>> Handle(RemoveRequestCommand command, CancellationToken token)
        {
            var request = await _context.Requests
                .FindAsync(command.RequestId);

            if (request == null)
                return Result<string>.Error("Заявка не найдена", Error.NotFound);

            if (request.ClientId != _currentUser.UserId)
                return Result<string>.Error("Это не ваша заявка", Error.Forbidden);

            if (request.Status == Domain.Enums.RequestStatus.InProgress ||
                request.Status == Domain.Enums.RequestStatus.Completed)
                return Result<string>.Error("Заявку отменить нельзя. Перезвоните, пожалуйста, водителю", Error.Conflict);

            request.Status = Domain.Enums.RequestStatus.Cancelled;

            await _context.SaveChangesAsync(token);

            return Result<string>.Success("OK");
        }
    }
}