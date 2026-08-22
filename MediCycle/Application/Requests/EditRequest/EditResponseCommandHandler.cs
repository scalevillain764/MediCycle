using Application.DTO.RequestDTO;
using Infrastructure;
using Infrastructure.Result;
using MediatR;
using ICurrentUser = Application.Abstractions.ICurrentUser;
using Error = Domain.Enums.ErrorType;
namespace Application.Requests.EditRequest
{
    public class EditResponseCommandHandler : IRequestHandler<EditRequestCommand, Result<RequestResponse>>
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUser _currentUser;
        public EditResponseCommandHandler(AppDbContext context, ICurrentUser currentUser)
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

            request.AddressId = command.AddressId;
            request.Weight = command.Weight;
            request.PreferredFromTime = command.PreferredFromTime;
            request.PreferredToTime = command.PreferredToTime;

            await _context.SaveChangesAsync(token);

            return Result<RequestResponse>.Success(new RequestResponse(request));
        }
    }
}
