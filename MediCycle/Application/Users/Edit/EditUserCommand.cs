using MediatR;
using Infrastructure.Responding;
using Application.DTO.UserDTO;
using Domain.Enums;
namespace Application.Users
{
    public record EditUserCommand(
        // client
        string? OrganizationName, 

        // worker
        string? Name, 
        string? Surname,
        DateTime? Birthday, 
        WorkerRole? Role, 
        string? DriverLicense) : IRequest<Result<UserResponse>>;
}