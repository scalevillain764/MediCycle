namespace Domain
{
    public class Address
    {
        public Ulid Id { get; private set; }

        public Ulid ClientId { get; set; }
        public Client? ClientOrganisation { get; set; }

        public string City { get; set; }
        public string Street { get; set; }
        public string BuildingNumber { get; set; } 
        
        public string PresentativeName { get; set; }
        public string PresentativeSurname { get; set; }
        public string PresentativePhone { get; set; }
        
        public ICollection<Request> Requests { get; set; } = [];

        public Address (Ulid clientId, string city, string street, string buldingNumber, string presentativeName,
            string presentativeSurname, string presentativePhone)
        {
            Id = Ulid.NewUlid();
            ClientId = clientId;
            City = city;
            Street = street;
            BuildingNumber = buldingNumber;
            PresentativeName = presentativeName;
            PresentativeSurname = presentativeSurname;
            PresentativePhone = presentativePhone;
        }
    }
}