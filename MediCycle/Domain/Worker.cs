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
        public Worker(string name, string surname, DateTime birthday, string passwordHash, 
            string refreshTokenHash, DateTime refreshTokenExpiresAt, WorkerRole role,
            string? driverLicenseNumber) : base(passwordHash, refreshTokenHash, refreshTokenExpiresAt)
        {
            Name = name;
            Surname = surname;
            Role = role;
            DriverLicenseNumber = driverLicenseNumber;
        }
    }
}