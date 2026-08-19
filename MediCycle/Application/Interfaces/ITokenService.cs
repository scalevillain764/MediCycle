using Domain;

namespace Application.Interfaces 
{
    public interface ITokenService
    {
        string CreateRefreshToken();
        string CreateAccessToken(Ulid Id, string name, string userType, string? userRole);
    }
}