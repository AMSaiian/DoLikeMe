using AMSaiian.Shared.Application.Wrappers;
using Taskio.Domain.Enums;

namespace Taskio.Common.Contract.Requests.Task;

public record UpdateTaskRequest
{
    private readonly string? _title;
    private readonly Undefinable<string?> _description;

    public string? Title
    {
        get => _title;
        init => _title = value?.Trim();
    }

    public Undefinable<string?> Description
    {
        get => _description;
        init => _description = new(value.IsDefined, value.Value?.Trim());
    }

    public Undefinable<DateTime?> DueDate { get; init; }
    public Status? Status { get; init; }
    public Priority? Priority { get; init; }
}
