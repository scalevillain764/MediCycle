namespace Application.DTO.AuthDTO
{
    public record  AuthLoginResponse(Ulid UserId, string accessToken);
}