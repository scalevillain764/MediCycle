using Domain.Enums;
namespace Application.DTO.UserDTO
{
    public record WorkerResponse(Ulid userId, string login, string name, string surname,
        DateTime birthday) : UserResponse(userId, login);
}