using Taskio.Domain.Enums;

namespace Taskio.Application.Common.Models.Task;

public record TaskShortDto
{
    public required Guid Id { get; init; }
    public required string Title { get; init; } = default!;
    public DateTime? DueDate { get; init; }
    public required Status Status { get; init; }
    public required Priority Priority { get; init; }
    public required Guid UserId { get; init; }
}
