namespace TaskTracker.Tests;

public class TaskTrackerTests : IDisposable
{
    private readonly string _filePath = "test_tasks.json";
    private readonly TaskManager _taskManager;

    public TaskTrackerTests()
    {
        _taskManager = new TaskManager(_filePath);
    }

    public void Dispose()
    {
        if (File.Exists(_filePath))
        {
            File.Delete(_filePath);
        }
    }

    [Fact]
    public void AddTask_ShouldCreateNewTask()
    {
        // Arrange
        var description = "Test Task";

        // Act
        var task = _taskManager.AddTask(description);

        // Assert
        Assert.NotNull(task);
        Assert.Equal(description, task.Description);
        Assert.Equal(TaskStatus.Todo, task.Status);
        Assert.NotEqual(default, task.CreatedAt);
        Assert.Null(task.UpdatedAt);
    }
}
