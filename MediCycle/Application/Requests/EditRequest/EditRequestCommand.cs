using Application.DTO.RequestDTO;
using Infrastructure.Result;
using MediatR;
namespace Application.Requests
{
    public record EditRequestCommand(Ulid RequestId,
        Ulid AddressId,
        bool MustCall,
        string? ShortDescription,
        decimal? Weight,
        DateTime? PreferredFromTime,
        DateTime? PreferredToTime) : IRequest<Result<RequestResponse>>;
}