using MediatR;
using Infrastructure.Responding;
using Application.DTO.UserDTO;
namespace Application.Users
{
    public record GetUserQuery(Ulid userId) : IRequest<Result<UserResponse>>;
}