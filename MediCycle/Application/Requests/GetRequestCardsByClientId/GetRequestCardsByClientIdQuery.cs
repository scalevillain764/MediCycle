using MediatR;
using Infrastructure.Responding;
using Application.DTO.RequestDTO;
namespace Application.Requests
{
    public record GetRequestCardsByClientIdQuery(Ulid ClientId, int Page = 1, int PageSize = 10) : IRequest<Result<PagedResponse<RequestResponseCard>>>;
}