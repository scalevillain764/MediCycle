namespace Application.Abstractions
{
    public interface ICurrentUser
    {
        Ulid? UserId { get; }
    }
}