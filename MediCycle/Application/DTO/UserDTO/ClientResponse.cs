using Application.DTO.UserDTO;
using Domain;
namespace Application.DTO.UserDTO
{
    public record ClientResponse(Ulid userId, string login, string organizationName)
        : UserResponse(userId, login)
    {
        public ClientResponse(Client c) : this(c.Id, c.Login, c.OrganizationName) { }
    }
}