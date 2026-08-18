namespace Domain
{
    public class Request {
        public Ulid Id { get; private set; }

        public Ulid ClientId { get; private set; }
        public Client? Client { get; private set; } = null!;

        public Ulid AddressId { get; private set; }
        public Address RequestAddress {get; private set;} // 1 : N

        public Ulid? ExecutorId { get; set; }
        public Worker? Executor { get; set; } = null;

        public decimal? Weight { get; set; } = null; // kg

        // preferred time
        public DateTime? PreferredFromTime { get; private set; }
        public DateTime? PreferredToTime { get; private set; }

        public Request(Ulid clientId, Ulid addressId, DateTime? preferredFromTime, DateTime? preferredToTime)
        {
            Id = Ulid.NewUlid();
            ClientId = clientId;
            AddressId = addressId;
            ExecutorId = null;
            Weight = null;
            PreferredFromTime = preferredFromTime;
            PreferredToTime = preferredToTime;
        }
    }
}