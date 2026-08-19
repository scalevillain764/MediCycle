namespace Domain
{
    public class Client : User
    {
        public string? OrganizationName { get; set; }
        public ICollection<Address> Addresses { get; set; } = [];
        public ICollection<Request> Requests { get; set; } = [];
        public Client(string login, string passwordHash)
            : base(login, passwordHash)
        {
            OrganizationName = null;
        }
    }
}