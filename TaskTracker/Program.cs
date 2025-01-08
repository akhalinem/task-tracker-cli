using TaskTracker;

class Program
{
    private static readonly TaskManager _taskManager = new();

    public static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            PrintUsage();
            return;
        }

        try
        {
            ProcessCommand(args);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private static void ProcessCommand(string[] args)
    {
        var command = args[0].ToLower();

        switch (command)
        {
            case "add" when args.Length >= 2:
                {
                    var description = string.Join(" ", args.Skip(1));
                    var newTask = _taskManager.AddTask(description);

                    Console.WriteLine($"Task added successfully (ID: {newTask.Id})");

                    break;
                }

            case "remove" when args.Length == 2:
                {
                    if (!Guid.TryParse(args[1], out var id))
                    {
                        Console.WriteLine("Invalid task ID.");
                        return;
                    }

                    var deleted = _taskManager.RemoveTask(id);
                    Console.WriteLine(deleted ? "Task deleted successfully" : "Task not found");

                    break;
                }

            case "update" when args.Length >= 3:
                {
                    if (!Guid.TryParse(args[1], out var taskId))
                    {
                        Console.WriteLine("Invalid task ID");
                        return;
                    }

                    var updateDescription = string.Join(" ", args.Skip(2));
                    var updatedTask = _taskManager.UpdateTask(taskId, updateDescription);
                    Console.WriteLine(updatedTask != null ? "Task updated successfully" : "Task not found");

                    break;
                }

            case "mark" when args.Length == 3:
                {
                    if (!Guid.TryParse(args[1], out var taskId))
                    {
                        Console.WriteLine("Invalid task ID");
                        return;
                    }

                    var status = ParseStatus(args[2]);

                    if (status == null)
                    {
                        Console.WriteLine("Invalid status. Use todo, in-progress, or done.");
                        return;
                    }

                    var markedTask = _taskManager.MarkTaskStatus(taskId, status.Value);

                    Console.WriteLine(markedTask != null ? "Task status updated successfully" : "Task not found");

                    break;
                }

            case "list":
                {
                    var status = args.Length == 2 ? ParseStatus(args[1]) : null;
                    var tasks = _taskManager.GetTasks(status);

                    PrintTasks(tasks);

                    break;
                }

            default:
                Console.WriteLine("Invalid command.");
                break;
        }
    }

    private static TaskTracker.TaskStatus? ParseStatus(string status)
    {
        return status.ToLower() switch
        {
            "todo" => TaskTracker.TaskStatus.Todo,
            "in-progress" => TaskTracker.TaskStatus.InProgress,
            "done" => TaskTracker.TaskStatus.Done,
            _ => null
        };
    }

    private static void PrintTasks(IEnumerable<TaskTracker.Task> tasks)
    {
        var taskList = tasks.ToList();
        if (taskList.Count == 0)
        {
            Console.WriteLine("No tasks found");
            return;
        }

        foreach (var task in taskList)
        {
            Console.WriteLine($"ID: {task.Id}");
            Console.WriteLine($"Description: {task.Description}");
            Console.WriteLine($"Status: {task.Status}");
            Console.WriteLine($"Created: {task.CreatedAt}");
            Console.WriteLine($"Updated: {task.UpdatedAt}");
            Console.WriteLine(new string('-', 40));
        }
    }

    private static void PrintUsage()
    {
        Console.WriteLine("Task Tracker CLI - Usage:");
        Console.WriteLine("  add <description>              - Add a new task");
        Console.WriteLine("  update <id> <description>      - Update an existing task");
        Console.WriteLine("  delete <id>                    - Delete a task");
        Console.WriteLine("  mark <id> <status>             - Mark task status (todo/in-progress/done)");
        Console.WriteLine("  list                           - List all tasks");
        Console.WriteLine("  list <status>                  - List tasks by status (todo/in-progress/done)");
        Console.WriteLine("\nExamples:");
        Console.WriteLine("  dotnet run -- add \"Buy groceries\"");
        Console.WriteLine("  dotnet run -- update 1 \"Buy groceries and cleaning supplies\"");
        Console.WriteLine("  dotnet run -- delete 1");
        Console.WriteLine("  dotnet run -- mark 1 in-progress");
        Console.WriteLine("  dotnet run -- mark 1 done");
        Console.WriteLine("  dotnet run -- list");
    }
}