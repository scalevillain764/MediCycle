using Domain;
using Infrastructure;
using Infrastructure.Result;
using MediatR;
using Address = Domain.Address;
using AddressResponse = Application.DTO.AddressDTO.AddressResponse;
using Error = Domain.Enums.ErrorType;
using ICurrentUser = Application.Abstractions.ICurrentUser;
namespace Application.Addresses
{
    public class RemoveAddressCommandHandler : IRequestHandler<RemoveAddressCommand, Result<string>>
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUser _currentUser;
        public RemoveAddressCommandHandler(AppDbContext context, ICurrentUser currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }
        public async Task<Result<string>> Handle(RemoveAddressCommand command, CancellationToken token)
        {
            var address = await _context.Addresses
                .FindAsync(command.addressId, token);

            if (address == null)
                return Result<string>.Error("Адрес не найден", Error.NotFound);

            if (_currentUser.UserType != "Client")
                return Result<string>.Error("Вы не можете удалить этот адрес", Error.Conflict);

            if (address.ClientId != _currentUser.UserId)
                return Result<string>.Error("Это не ваш адрес", Error.Forbidden);

            _context.Addresses.Remove(address);

            await _context.SaveChangesAsync(token);

            return Result<string>.Success("OK");
        }
    }
}