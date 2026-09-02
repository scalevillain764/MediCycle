using Application.DTO.RequestDTO;
using Infrastructure;
using Infrastructure.Responding;
using MediatR;
using Request = Domain.Request;
using Error = Domain.Enums.ErrorType;
using Microsoft.EntityFrameworkCore;
using ICurrentUser = Application.Abstractions.ICurrentUser;
namespace Application.Requests
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
            var address = await _context.Addresses
                .Where(x => x.ClientId == _currentUser.UserId)
                .FirstOrDefaultAsync(x => x.Id == command.AddressId, token);

            if (address == null)
                return Result<RequestResponse>.Error("Такой адрес не найден", Error.NotFound);

            var request = new Request((Ulid)_currentUser.UserId, address.Id, command.MustCall, command.ShortDescription, command.Weight, command.PreferredFromTime, command.PreferredToTime);

            _context.Requests.Add(request);

            await _context.SaveChangesAsync(token);

            var request_rez = await _context.Requests
                .Include(x => x.RequestAddress)
                .Include(x => x.Client)
                .Include(x => x.Executor)
                .FirstOrDefaultAsync(x => x.Id == request.Id, token);

            if (request_rez == null)
                return Result<RequestResponse>.Error("Что-то пошло не так", Error.Conflict);

            return Result<RequestResponse>.Success(new RequestResponse(request_rez));
        }
    }
}