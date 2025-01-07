using TaskManager;

class Program
{
    private static readonly TaskManager.TaskManager _taskManager = new();

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
                var description = string.Join(" ", args.Skip(1));
                var newTask = _taskManager.AddTask(description);

                Console.WriteLine($"Task added successfully (ID: {newTask.Id})");

                break;

            case "remove":
                // TODO: Implement
                break;

            case "update":
                // TODO: Implement
                break;

            case "list":
                var status = args.Length == 2 ? ParseStatus(args[1]) : null;
                var tasks = _taskManager.GetTasks(status);

                PrintTasks(tasks);

                break;

            default:
                Console.WriteLine("Invalid command.");
                break;
        }
    }

    private static TaskManager.TaskStatus? ParseStatus(string status)
    {
        return status.ToLower() switch
        {
            "todo" => TaskManager.TaskStatus.Todo,
            "in-progress" => TaskManager.TaskStatus.InProgress,
            "done" => TaskManager.TaskStatus.Done,
            _ => null
        };
    }

    private static void PrintTasks(IEnumerable<TaskManager.Task> tasks)
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
        Console.WriteLine("  add <description>            - Add a new task");
        Console.WriteLine("  list                         - List all tasks");
        Console.WriteLine("  list <status>                - List tasks by status (todo/in-progress/done)");
    }
}