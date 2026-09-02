using Application.DTO.RequestDTO;
using MediatR;
using Infrastructure.Responding;
namespace Application.Requests
{
    public record AddRequestCommand(
        Ulid AddressId,
        bool MustCall,
        string? ShortDescription, 
        decimal? Weight,
        DateTime? PreferredFromTime,
        DateTime? PreferredToTime
        ) : IRequest<Result<RequestResponse>>;
}