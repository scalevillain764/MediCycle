using MediatR;
using Application.DTO.RequestDTO;
using Infrastructure.Responding;
namespace Application.DTO.RequestDTO
{
    public record GetRequestCardsByExecutorIdQuery(Ulid executorId, int page, int pageSize) : IRequest<Result<PagedResponse<RequestResponseCard>>>;
}