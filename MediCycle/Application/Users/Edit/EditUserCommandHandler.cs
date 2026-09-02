using MediatR;
using Application.DTO.UserDTO;
using ICurrentUser = Application.Abstractions.ICurrentUser;
using Infrastructure.Responding;
using Infrastructure;
using Domain;
using Microsoft.EntityFrameworkCore;
namespace Application.Users
{
    public class EditUserCommandHandler : IRequestHandler<EditUserCommand, Result<UserResponse>>
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUser _currentUser;
        public EditUserCommandHandler(AppDbContext context, ICurrentUser currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }
        public async Task<Result<UserResponse>> Handle(EditUserCommand command, CancellationToken token)
        {
            var user = await _context.AllUsers
                .FindAsync(_currentUser.UserId, token);

            if (user == null)
                return Result<UserResponse>.Error("Что-то пошло не так", Domain.Enums.ErrorType.Conflict);

            switch (user)
            {
                case Worker w:
                    {
                        w.Name = command.Name;
                        w.Surname = command.Surname;
                        w.Birthday = command.Birthday != null ? (DateTime)command.Birthday : w.Birthday;
                        w.DriverLicenseNumber = command.DriverLicense;
                        break;
                    }
                case Client c:
                    {
                        c.OrganizationName = command.OrganizationName;
                        break;
                    }
            }

            await _context.SaveChangesAsync(token);

            if (user is Worker worker)
                return Result<UserResponse>.Success(new WorkerResponse(worker));
            
            if(user is Client client)
                return Result<UserResponse>.Success(new ClientResponse(client));

            return Result<UserResponse>.Error("Что-то пошло не так", Domain.Enums.ErrorType.Conflict);
        }
    }
}