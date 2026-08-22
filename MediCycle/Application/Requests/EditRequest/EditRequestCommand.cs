using Application.DTO.RequestDTO;
using Infrastructure.Result;
using MediatR;
namespace Application.Requests.EditRequest
{
    public record EditRequestCommand(Ulid RequestId,
        Ulid AddressId,
        decimal? Weight,
        DateTime? PreferredFromTime,
        DateTime? PreferredToTime) : IRequest<Result<RequestResponse>>;
}