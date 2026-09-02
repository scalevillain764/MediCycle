using Domain;
using MediatR;
using Infrastructure.Responding;
namespace Application.DTO.RequestDTO
{
    public record StartCompletingRequestCommand(Ulid requestId) : IRequest<Result<RequestResponse>>;
}
