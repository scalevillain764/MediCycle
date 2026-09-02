using Domain;
using MediatR;
using Infrastructure.Responding;
using Application.DTO.RequestDTO;
namespace Application.Requests
{
    public record AssignRequestCommand(Ulid requestId, Ulid executorId) : IRequest<Result<RequestResponse>>;
}