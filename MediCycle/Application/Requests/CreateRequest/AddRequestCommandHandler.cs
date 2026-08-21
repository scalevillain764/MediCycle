using Application.DTO.RequestDTO;
using Infrastructure;
using Infrastructure.Result;
using MediatR;
using Request = Domain.Request;
using Error = Domain.Enums.ErrorType;
using Microsoft.EntityFrameworkCore;
using AddRequestCommand = Application.Requests.CreateRequest.AddRequestCommand;
using ICurrentUser = Application.Abstractions.ICurrentUser;
namespace Application.Requests.CreateRequest
{
    public class CreateRequestCommandHandler : IRequestHandler<AddRequestCommand, Result<RequestResponse>>
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUser _currentUser;
        public CreateRequestCommandHandler(AppDbContext context, ICurrentUser currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }
        public async Task<Result<RequestResponse>> Handle(AddRequestCommand command, CancellationToken token)
        {
            if (_currentUser.UserId == null)
                return Result<RequestResponse>.Error("Пользователь не авторизован", Error.Unauthorized);

            var address = await _context.Addresses
                .Where(x => x.ClientId == _currentUser.UserId)
                .FirstOrDefaultAsync(x => x.Id == command.AddressId, token);

            if (address == null)
                return Result<RequestResponse>.Error("Такой адрес не найден", Error.NotFound);

            var request = new Request((Ulid)_currentUser.UserId, address.Id, command.Weight, command.PreferredFromTime, command.PreferredToTime);

            _context.Requests.Add(request);

            await _context.SaveChangesAsync(token);

            return Result<RequestResponse>.Success(new RequestResponse(request));
        }
    }
}