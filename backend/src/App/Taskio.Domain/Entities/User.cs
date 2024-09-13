namespace Taskio.Domain.Entities;

public class User : BaseEntity
{
    public Guid AuthId { get; set; }

    public List<Task> Tasks
    {
        get { return _tasks ??= []; }
        set => _tasks = value;
    }

    private List<Task>? _tasks;
}
