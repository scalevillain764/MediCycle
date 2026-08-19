namespace Domain
{
    public abstract class User
    {
        public Ulid Id { get; private set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public string? RefreshTokenHash { get; set; }
        public DateTime? RefreshTokenExpiresAt { get; set; }
        public User(string login, string passwordHash)
        {
            Id = Ulid.NewUlid();
            Login = login;
            PasswordHash = passwordHash;
            RefreshTokenHash = null;
            RefreshTokenExpiresAt = null;
        }
    }
}