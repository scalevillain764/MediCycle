using Request = Domain.Request;
namespace Application.DTO.RequestDTO
{
    public record RequestResponseCard(string organizationName,
        string street, 
        string city,
        string buildingNumber,
        DateTime CreatedAt)
    {
        public RequestResponseCard(Request request)
            : this(request.Client.OrganizationName,
                  request.RequestAddress.City,
                  request.RequestAddress.Street,
                  request.RequestAddress.BuildingNumber,
                  request.CreatedAt)
        { }
    }
}