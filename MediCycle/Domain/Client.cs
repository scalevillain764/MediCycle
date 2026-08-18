namespace Domain
{
    public class Client : User
    {
        public string OrganizationName { get; set; }
        public ICollection<Address> Adresses { get; set; } = [];
        public ICollection<Request> Requests { get; set; } = [];
        public Client(string organizationName, string passwordHash,
            string refreshTokenHash, DateTime refreshTokenExpiresAt)
            : base(passwordHash, refreshTokenHash, refreshTokenExpiresAt)
        {
            OrganizationName = organizationName;
        }
    }
}