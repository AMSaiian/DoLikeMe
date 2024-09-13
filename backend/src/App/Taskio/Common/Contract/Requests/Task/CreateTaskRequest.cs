using System.Diagnostics.CodeAnalysis;
using Taskio.Domain.Enums;

namespace Taskio.Common.Contract.Requests.Task;

public record CreateTaskRequest
{
    private readonly string _title;
    private readonly string? _description;

    public required string Title
    {
        get => _title;
        [MemberNotNull(nameof(_title))] init => _title = value.Trim();
    }

    public string? Description
    {
        get => _description;
        init => _description = value?.Trim();
    }

    public DateTime? DueDate { get; init; }
    public required Status Status { get; init; }
    public required Priority Priority { get; init; }
    public required Guid UserId { get; init; }
};
