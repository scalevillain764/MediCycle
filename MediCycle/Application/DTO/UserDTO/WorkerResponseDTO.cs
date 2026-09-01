using Domain.Enums;
using Domain;
namespace Application.DTO.UserDTO
{
    public record WorkerResponse(Ulid userId, string login, string name, string surname,
        DateTime birthday) : UserResponse(userId, login)
    {
        public WorkerResponse(Worker w) : this(w.Id, w.Login, w.Name, w.Surname, w.Birthday) { }
    }
}