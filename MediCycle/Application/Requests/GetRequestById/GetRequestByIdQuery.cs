using Application.DTO.RequestDTO;
using Infrastructure.Result;
using MediatR;
namespace Application.Requests.GetRequest
{
    public record GetRequestByIdQuery(Ulid RequestId) : IRequest<Result<RequestResponse>>;
}