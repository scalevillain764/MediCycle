using Application.DTO.AuthDTO.Client;
using Domain.Enums;

namespace Application.DTO.AuthDTO.Worker
{
    public record AuthWorkerRegistrationDTO(string login, string password,
        string name, string surname, DateTime birthday, WorkerRole role,
        string? driverLicenseNumber) : UniversalRegistrationDTO(login, password);
}