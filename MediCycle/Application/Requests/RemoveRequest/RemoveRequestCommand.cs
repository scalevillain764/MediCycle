using MediatR;
using Infrastructure.Result;
namespace Application.Requests.RemoveRequest
{
    public record RemoveRequestCommand(Ulid CommandRequest) : IRequest<Result<string>>;
}