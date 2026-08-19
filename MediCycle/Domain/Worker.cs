using WorkerRole = Domain.Enums.WorkerRole;
namespace Domain
{
    public class Worker : User
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime Birthday { get; set; }
        public WorkerRole Role { get; set; }
        public string? DriverLicenseNumber { get; set; }
        public ICollection<Request> Requests { get; set; } = [];
        public Worker(string login, string passwordHash, string name, string surname,
            DateTime birthday, WorkerRole role, string? driverLicense) : base(login, passwordHash)
        {
            Name = name;
            Surname = surname;
            Role = role;
            DriverLicenseNumber = driverLicense;
        }
    }
}