using System.Text.Json;

class TaskManager
{
    private readonly string _filePath;
    private readonly List<Task> _tasks = [];

    public TaskManager(string filePath = "tasks.json")
    {
        _filePath = filePath;
        _tasks = LoadTasks();
    }

    private List<Task> LoadTasks()
    {
        if (!File.Exists(_filePath))
        {
            return [];
        }

        try
        {
            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<Task>>(json) ?? [];
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading tasks: {ex.Message}");
            return [];
        }
    }

    private void SaveTasks()
    {
        try
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(_tasks, options);
            File.WriteAllText(_filePath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving tasks: {ex.Message}");
        }
    }

    public Task AddTask(string description)
    {
        var task = new Task
        {
            Id = Guid.NewGuid(),
            Description = description,
        };

        _tasks.Add(task);
        SaveTasks();

        return task;
    }

    public bool RemoveTask(Guid id)
    {
        var task = _tasks.FirstOrDefault(t => t.Id == id);
        if (task == null) return false;

        _tasks.Remove(task);
        SaveTasks();

        return true;
    }

    public Task? UpdateTask(Guid id, string description)
    {
        var task = _tasks.FirstOrDefault(t => t.Id == id);
        if (task == null) return null;

        task.Description = description;
        task.UpdatedAt = DateTime.UtcNow;

        SaveTasks();

        return task;
    }

    public Task? MarkTaskStatus(Guid id, TaskStatus status)
    {
        var task = _tasks.FirstOrDefault(t => t.Id == id);
        if (task == null) return null;

        task.Status = status;
        task.UpdatedAt = DateTime.Now;

        SaveTasks();

        return task;
    }

    public List<Task> GetTasks(TaskStatus? status = null)
    {
        return status.HasValue
            ? _tasks.Where(t => t.Status == status).ToList()
            : _tasks;
    }
}
