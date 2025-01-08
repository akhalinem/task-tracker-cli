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
