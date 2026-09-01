using Application.DTO.UserDTO;
namespace Application.DTO.UserDTO
{
    public record ClientResponse(Ulid userId, string login, string organizationName)
        : UserResponse(userId, login);
}