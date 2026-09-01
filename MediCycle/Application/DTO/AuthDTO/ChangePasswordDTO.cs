namespace Application.DTO.AuthDTO
{
    public record ChangePasswordDTO(string newPassword, string oldPassword);
}