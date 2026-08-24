using MediatR;
using Infrastructure.Result;
using Application.DTO.RequestDTO;
namespace Application.Requests
{
    public record GetRequestCardsByClientIdQuery(Ulid ClientId) : IRequest<Result<List<RequestResponseCard>>>;
}