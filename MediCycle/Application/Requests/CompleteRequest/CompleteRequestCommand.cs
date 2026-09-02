using Infrastructure.Result;
using MediatR;
using Application.DTO.RequestDTO;
namespace Application.Requests
{
    public record CompleteRequestCommand(Ulid requestId): IRequest<Result<RequestResponse>>;
}