using MediatR;
using Infrastructure.Result;
using Application.DTO.UserDTO;
namespace Application.Users
{
    public record GetUserQuery(Ulid userId) : IRequest<Result<UserResponse>>;
}