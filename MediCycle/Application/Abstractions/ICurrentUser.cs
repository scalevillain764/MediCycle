namespace Application.Abstractions
{
    public interface ICurrentUser
    {
        Ulid? UserId { get; }
        string? UserType { get; }
        string? UserRole { get; }
    }
}