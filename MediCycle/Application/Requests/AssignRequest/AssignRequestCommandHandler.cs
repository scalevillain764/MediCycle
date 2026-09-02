using Application.DTO.RequestDTO;
using Infrastructure;
using Infrastructure.Result;
using MediatR;
using Domain;
using Request = Domain.Request;
using Error = Domain.Enums.ErrorType;
using RequestStatus = Domain.Enums.RequestStatus;
using WorkerRole = Domain.Enums.WorkerRole;
using Microsoft.EntityFrameworkCore;
using ICurrentUser = Application.Abstractions.ICurrentUser;
namespace Application.Requests
{
    public class AssignRequestCommandHandler : IRequestHandler<AssignRequestCommand, Result<RequestResponse>>
    {
        private readonly ICurrentUser _currentUser;
        private readonly AppDbContext _context;
        public AssignRequestCommandHandler(ICurrentUser currentUser, AppDbContext context)
        {
            _currentUser = currentUser;
            _context = context;
        }
        public async Task<Result<RequestResponse>> Handle(AssignRequestCommand command, CancellationToken token)
        {
            var request = await _context.Requests
                .Include(x => x.RequestAddress)
                .Include(x => x.Client)
                .FirstOrDefaultAsync(x => x.Id == command.requestId, token);

            if (request == null)
                return Result<RequestResponse>.Error("Заявка не найдена", Error.NotFound);

            if (request.Status != RequestStatus.Created && request.Status != RequestStatus.Assigned)
                return Result<RequestResponse>.Error("Эту заявку нельзя назначить", Error.Forbidden);

            if (_currentUser.UserRole is not "Dispatcher")
                return Result<RequestResponse>.Error("Заявки могут назначать только диспетчеры", Error.Conflict);

            var executor = await _context.Workers
                .FindAsync(command.executorId, token);

            if(executor == null)
                 return Result<RequestResponse>.Error("Водитель не найден", Error.NotFound);

            if (executor.Role != WorkerRole.Driver)
                return Result<RequestResponse>.Error("Заявки можно назначить тольк водителям", Error.Conflict);

            if (executor.DriverLicenseNumber == null)
                return Result<RequestResponse>.Error("Пока водитель не указал свое ВУ, нельзя", Error.Forbidden);

            request.Status = RequestStatus.Assigned;
            request.ExecutorId = command.executorId;

            await _context.SaveChangesAsync(token);
            await _context.Entry(request).Reference(x => x.Executor).LoadAsync(token);

            return Result<RequestResponse>.Success(new RequestResponse(request));
        }
    }
}