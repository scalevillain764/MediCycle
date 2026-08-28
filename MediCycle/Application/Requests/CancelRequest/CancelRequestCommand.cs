using Application.DTO.RequestDTO;
using MediatR;
using Infrastructure.Result;
namespace Application.Requests
{
    public record CancelRequestCommand(Ulid RequestId) : IRequest<Result<RequestResponse>>;
}