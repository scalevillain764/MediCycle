using MediatR;
using Infrastructure.Responding;
using Application.DTO.RequestDTO;
namespace Application.Requests
{
    public record GetRequestCardsByClientIdQuery(Ulid ClientId, int Page, int PageSize) : IRequest<Result<PagedResponse<RequestResponseCard>>>;
}