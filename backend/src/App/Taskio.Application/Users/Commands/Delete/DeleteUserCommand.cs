using MediatR;

namespace Taskio.Application.Users.Commands.Delete;

public record DeleteUserCommand : IRequest
{
    public required Guid Id { get; init; }
}

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
{
    public System.Threading.Tasks.Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
