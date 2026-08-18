namespace Domain
{
    public abstract class User
    {
        public Ulid Id { get; private set; }
        public string PasswordHash { get; set; }
        public string RefreshTokenHash { get; set; }
        public DateTime RefreshTokenExpiresAt { get; set; }
        public User(string passwordHash, string refreshTokenHash, DateTime refreshTokenExpiresAt)
        {
            Id = Ulid.NewUlid();
            PasswordHash = passwordHash;
            RefreshTokenHash = refreshTokenHash;
            RefreshTokenExpiresAt = refreshTokenExpiresAt;
        }
    }
}