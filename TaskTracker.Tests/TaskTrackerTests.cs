using System.Text.Json;

namespace TaskTracker.Tests;

public class TaskTrackerTests : IDisposable
{
    private readonly string _testFilePath = "test_tasks.json";
    private readonly TaskManager _taskManager;

    public TaskTrackerTests()
    {
        _taskManager = new TaskManager(_testFilePath);
    }

    public void Dispose()
    {
        if (File.Exists(_testFilePath))
        {
            File.Delete(_testFilePath);
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

    [Fact]
    public void AddTask_ShouldPersistToFile()
    {
        // Arrange
        var description = "Test task";

        // Act
        _taskManager.AddTask(description);
        var json = File.ReadAllText(_testFilePath);
        var tasks = JsonSerializer.Deserialize<List<Task>>(json);

        // Assert
        Assert.True(File.Exists(_testFilePath));
        Assert.NotNull(tasks);
        Assert.Single(tasks);
        Assert.Equal(description, tasks[0].Description);
    }

    [Fact]
    public void RemoveTask_ShouldRemoveExistingTask()
    {
        // Arrange
        var task = _taskManager.AddTask("Test task");

        // Act
        var result = _taskManager.RemoveTask(task.Id);

        // Assert
        Assert.True(result);
        Assert.Empty(_taskManager.GetTasks());
    }

    [Fact]
    public void RemoveTask_ShouldReturnFalseForNonExistentTask()
    {
        // Act
        var result = _taskManager.RemoveTask(Guid.NewGuid());

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void UpdateTask_ShouldUpdateExistingTask()
    {
        // Arrange
        var task = _taskManager.AddTask("Original description");
        var newDescription = "Updated description";

        // Act
        var updatedTask = _taskManager.UpdateTask(task.Id, newDescription);

        // Assert

        Assert.NotNull(updatedTask);
        Assert.Equal(newDescription, updatedTask.Description);
        Assert.NotNull(updatedTask.UpdatedAt);
    }

    [Fact]
    public void UpdateTask_ShouldReturnNullForNonExistentTask()
    {
        // Act
        var result = _taskManager.UpdateTask(Guid.NewGuid(), "New description");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void MarkTaskStatus_ShouldReturnNullForNonexistentTask()
    {
        // Act
        var result = _taskManager.MarkTaskStatus(Guid.NewGuid(), TaskStatus.Done);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetTasks_ShouldReturnAllTasksWhenNoStatusSpecified()
    {
        // Arrange
        _taskManager.AddTask("Task 1");
        _taskManager.AddTask("Task 2");

        // Act
        var tasks = _taskManager.GetTasks();

        // Assert
        Assert.Equal(2, tasks.Count);
    }

    [Fact]
    public void GetTasks_ShouldFilterByStatus()
    {
        // Arrange
        var task1 = _taskManager.AddTask("Task 1");
        var task2 = _taskManager.AddTask("Task 2");
        _taskManager.MarkTaskStatus(task1.Id, TaskStatus.Done);

        // Act
        var doneTasks = _taskManager.GetTasks(TaskStatus.Done);
        var todoTasks = _taskManager.GetTasks(TaskStatus.Todo);

        // Assert
        Assert.Single(doneTasks);
        Assert.Single(todoTasks);
        Assert.Equal(task1.Id, doneTasks[0].Id);
        Assert.Equal(task2.Id, todoTasks[0].Id);
    }
}
