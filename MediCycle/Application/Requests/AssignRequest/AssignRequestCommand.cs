using Domain;
using MediatR;
using Infrastructure.Result;
namespace Application.DTO.RequestDTO
{
    public record AssignRequestCommand(Ulid requestId, Ulid executorId) : IRequest<Result<RequestResponse>>;
}