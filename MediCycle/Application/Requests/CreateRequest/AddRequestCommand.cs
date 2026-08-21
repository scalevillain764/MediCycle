using Application.DTO.RequestDTO;
using MediatR;
using Infrastructure.Result;
namespace Application.Requests.CreateRequest
{
    public record AddRequestCommand(
        Ulid AddressId,
        decimal? Weight,
        DateTime? PreferredFromTime,
        DateTime? PreferredToTime
        ) : IRequest<Result<RequestResponse>>;
}