using MediatR;
using ICurrentUser = Application.Abstractions.ICurrentUser;
using AddressResponse = Application.DTO.AddressDTO.AddressResponse;
using Address = Domain.Address;
using Infrastructure.Responding;
using Infrastructure;
using Error = Domain.Enums.ErrorType;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
namespace Application.Addresses
{
    public class GetAddressesByClientIdQueryHandler : IRequestHandler<GetAddressesByClientIdQuery, Result<List<AddressResponse>>>
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUser _currentUser;
        public GetAddressesByClientIdQueryHandler(AppDbContext context, ICurrentUser currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }
        public async Task<Result<List<AddressResponse>>> Handle(GetAddressesByClientIdQuery query, CancellationToken token)
        {
            Ulid? clientId = _currentUser.UserType is "Client" ? _currentUser.UserId : query.clientId;

            if (clientId == null)
                return Result<List<AddressResponse>>.Error("Ошибка авторизации", Error.Unauthorized);

            var rez = await _context.Addresses
                .Where(x => x.ClientId == clientId)
                .Select(x => new AddressResponse(x))
                .AsNoTracking()
                .ToListAsync(token);

            return Result<List<AddressResponse>>.Success(rez);
        }
    }
}