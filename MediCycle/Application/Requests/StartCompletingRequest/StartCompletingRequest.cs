using Domain;
using MediatR;
using Infrastructure.Result;
namespace Application.DTO.RequestDTO
{
    public record StartCompletingRequestCommand(Ulid requestId) : IRequest<Result<RequestResponse>>;
}
