using Application.DTO.RequestDTO;
using Infrastructure.Responding;
using MediatR;
namespace Application.Requests
{
    public record GetRequestByIdQuery(Ulid RequestId) : IRequest<Result<RequestResponse>>;
}