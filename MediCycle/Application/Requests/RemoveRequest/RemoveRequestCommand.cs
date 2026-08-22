using MediatR;
using Infrastructure.Result;
namespace Application.Requests.RemoveRequest
{
    public record RemoveRequestCommand(Ulid RequestId) : IRequest<Result<string>>;
}