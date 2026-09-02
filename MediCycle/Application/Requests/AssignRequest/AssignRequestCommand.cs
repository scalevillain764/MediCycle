using Domain;
using MediatR;
using Infrastructure.Responding;
namespace Application.DTO.RequestDTO
{
    public record AssignRequestCommand(Ulid requestId, Ulid executorId) : IRequest<Result<RequestResponse>>;
}