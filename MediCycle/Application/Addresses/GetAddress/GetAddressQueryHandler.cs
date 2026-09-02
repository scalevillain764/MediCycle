using MediatR;
using ICurrentUser = Application.Abstractions.ICurrentUser;
using AddressResponse = Application.DTO.AddressDTO.AddressResponse;
using Address = Domain.Address;
using Infrastructure.Result;
using Infrastructure;
using Error = Domain.Enums.ErrorType;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
namespace Application.Addresses
{
    public class GetAddressQueryHandler : IRequestHandler<GetAddressQuery, Result<AddressResponse>>
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUser _currentUser;
        public GetAddressQueryHandler(AppDbContext context, ICurrentUser currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }
        public async Task<Result<AddressResponse>> Handle(GetAddressQuery query, CancellationToken token)
        {
            var address = await _context.Addresses
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == query.addressId, token);

            if (address == null)
                return Result<AddressResponse>.Error("Адрес не найден", Error.NotFound);

            if(_currentUser.UserType is "Client")
            {
                if (_currentUser.UserId != address.ClientId)
                    return Result<AddressResponse>.Error("Вы не можете увидеть этот адрес", Error.Forbidden);
            }
            
            return Result<AddressResponse>.Success(new AddressResponse(address));
        }
    }
}