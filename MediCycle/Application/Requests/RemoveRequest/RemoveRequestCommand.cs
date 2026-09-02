using MediatR;
using Infrastructure.Responding;
namespace Application.Requests 
{ 
    public record RemoveRequestCommand(Ulid RequestId) : IRequest<Result<string>>;
}