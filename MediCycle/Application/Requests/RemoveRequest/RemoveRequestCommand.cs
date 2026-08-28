using MediatR;
using Infrastructure.Result;
namespace Application.Requests 
{ 
    public record RemoveRequestCommand(Ulid RequestId) : IRequest<Result<string>>;
}