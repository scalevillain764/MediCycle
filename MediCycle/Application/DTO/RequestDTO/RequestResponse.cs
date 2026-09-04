using Request = Domain.Request;
namespace Application.DTO.RequestDTO
{
    public record RequestResponse(
        string organizationName,
        bool mustCall,
        string? shortDescription,
        string street,
        string city,
        string buildingNumber,
        string presentativeName,
        string presentativeSurname,
        string represetativeNumber,
        string requestStatus,
        decimal? requestWeight,
        string? executorName,
        string? executorSurname,
        DateTime createdAt,
        DateTime? preferredFromTime,
        DateTime? preferredToTime)
    {
        public RequestResponse(Request request)
            : this(
                  request.Client?.OrganizationName,
                  request.MustCall,
                  request.ShortDescription,
                  request.RequestAddress.Street,
                  request.RequestAddress.City,
                  request.RequestAddress.BuildingNumber,
                  request.RequestAddress.PresentativeName,
                  request.RequestAddress.PresentativeSurname,
                  request.RequestAddress.PresentativePhone,
                  request.Status.ToString(),
                  request.Weight,
                  request.Executor?.Name,
                  request.Executor?.Surname,
                  request.CreatedAt,
                  request.PreferredFromTime,
                  request.PreferredToTime)
        { }
    }
}