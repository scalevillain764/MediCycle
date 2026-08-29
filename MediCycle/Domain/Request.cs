using Status = Domain.Enums.RequestStatus;
namespace Domain
{
    public class Request {
        public Ulid Id { get; private set; }

        public Ulid ClientId { get; set; }
        public Client? Client { get; set; } = null!;

        public Ulid AddressId { get; set; }
        public Address RequestAddress { get; set; } = null!; // 1 : N

        public Ulid? ExecutorId { get; set; }
        public Worker? Executor { get; set; } = null;

        public bool MustCall { get; set; }
        public string? ShortDescription { get; set; }
        public decimal? Weight { get; set; } = null; // kg
        public Status Status { get; set; }
        public DateTime CreatedAt { get; private set; }

        // preferred time
        public DateTime? PreferredFromTime { get; set; }
        public DateTime? PreferredToTime { get; set; }

        public Request(Ulid clientId, Ulid addressId, bool mustCall, string? shortDescription, decimal? weight, DateTime? preferredFromTime, DateTime? preferredToTime)
        {
            Id = Ulid.NewUlid();
            ClientId = clientId;
            AddressId = addressId;
            MustCall = mustCall;
            ShortDescription = shortDescription;
            Weight = weight;
            ExecutorId = null;
            CreatedAt = DateTime.UtcNow;
            PreferredFromTime = preferredFromTime;
            PreferredToTime = preferredToTime;
            Status = Status.Created;
        }
    }
}