namespace Application.DTO.AuthDTO.Client
{
    public record AuthClientRegistrationDTO(string login, string password) : UniversalRegistrationDTO(login, password);
}