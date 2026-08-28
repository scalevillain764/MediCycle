namespace Application.DTO.AuthDTO.Client
{
    public record AuthClientRegistrationDTO(string organization_name, string login, string password) : UniversalRegistrationDTO(login, password);
}