using Application.DTO.RequestDTO;
using Infrastructure.Responding;
using MediatR;
namespace Application.Requests
{
    public record EditRequestCommand(
        Ulid AddressId,
        bool MustCall,
        string? ShortDescription,
        decimal? Weight,
        DateTime? PreferredFromTime,
        DateTime? PreferredToTime,
        Ulid RequestId = default) : IRequest<Result<RequestResponse>>;
}