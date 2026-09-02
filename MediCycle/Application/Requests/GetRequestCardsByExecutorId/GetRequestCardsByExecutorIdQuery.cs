using MediatR;
using Application.DTO.RequestDTO;
using Infrastructure.Result;
namespace Application.DTO.RequestDTO
{
    public record GetRequestCardsByExecutorIdQuery(Ulid executorId) : IRequest<Result<List<RequestResponseCard>>;
}