namespace Taskio.Application.Common.Models.Task;

public record TaskFullDto : TaskShortDto
{
    public string? Description { get; init; }
}
