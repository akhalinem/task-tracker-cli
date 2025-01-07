namespace TaskManager;

enum TaskStatus
{
    Todo,
    InProgress,
    Done
}

class Task
{
    public Guid Id { get; set; }
    public string Description { get; set; } = "";
    public TaskStatus Status { get; set; } = TaskStatus.Todo;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; } = null;
}

class TaskManager
{
    private readonly List<Task> _tasks = [];

    public Task AddTask(string description)
    {
        var task = new Task
        {
            Id = Guid.NewGuid(),
            Description = description,
        };

        _tasks.Add(task);

        return task;
    }

    public bool RemoveTask(Guid id)
    {
        var task = _tasks.FirstOrDefault(t => t.Id == id);
        if (task == null) return false;

        _tasks.Remove(task);

        return true;
    }

    public Task? UpdateTask(Guid id, string description)
    {
        var task = _tasks.FirstOrDefault(t => t.Id == id);
        if (task == null) return null;

        task.Description = description;
        task.UpdatedAt = DateTime.UtcNow;
        return task;
    }

    public List<Task> GetTasks(TaskStatus? status = null)
    {
        return status.HasValue
            ? _tasks.Where(t => t.Status == status).ToList()
            : _tasks;
    }
}
