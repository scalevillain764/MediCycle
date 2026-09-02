using Application.DTO.RequestDTO;
using Infrastructure;
using Infrastructure.Result;
using MediatR;
using ICurrentUser = Application.Abstractions.ICurrentUser;
using Error = Domain.Enums.ErrorType;
using Microsoft.EntityFrameworkCore;
namespace Application.Requests
{
    public class EditRequestCommandHandler : IRequestHandler<EditRequestCommand, Result<RequestResponse>>
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUser _currentUser;
        public EditRequestCommandHandler(AppDbContext context, ICurrentUser currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }
        public async Task<Result<RequestResponse>> Handle(EditRequestCommand command, CancellationToken token)
        {
            var request = await _context.Requests
                .FindAsync(command.RequestId, token);

            if (request == null)
                return Result<RequestResponse>.Error("Заявка не найдена", Error.NotFound);

            if (request.ClientId != _currentUser.UserId)
                return Result<RequestResponse>.Error("Это не ваша заявка", Error.Forbidden);

            var address = await _context.Addresses
                .Where(x => x.ClientId == _currentUser.UserId)
                .FirstOrDefaultAsync(x => x.Id == command.AddressId, token);

            if (address == null)
                return Result<RequestResponse>.Error("Такой адрес не найден", Error.NotFound);

            request.AddressId = command.AddressId;
            request.ShortDescription = command.ShortDescription;
            request.MustCall = command.MustCall;
            request.Weight = command.Weight;
            request.PreferredFromTime = command.PreferredFromTime;
            request.PreferredToTime = command.PreferredToTime;

            await _context.SaveChangesAsync(token);

            await _context.Entry(request).Reference(x => x.RequestAddress).LoadAsync(token);
            await _context.Entry(request).Reference(x => x.Client).LoadAsync(token);
            await _context.Entry(request).Reference(x => x.Executor).LoadAsync(token);

            return Result<RequestResponse>.Success(new RequestResponse(request));
        }
    }
}
