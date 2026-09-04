using MediatR;
using Infrastructure.Responding;
using Application.DTO.RequestDTO;
namespace Application.Requests
{
    public record GetNotAssignedRequestsQuery(string status, int page, int pageSize) : IRequest<Result<PagedResponse<RequestResponseCard>>;
}