namespace Application.Interfaces 
{
    public interface ITokenService
    {
        public string CreateRefreshToken();
        public string CreateAccessToken(Ulid Id, string userName);
    }
}