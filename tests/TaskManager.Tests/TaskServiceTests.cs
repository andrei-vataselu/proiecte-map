using TaskManager.Core.Interfaces;
using TaskManager.Core.Models;
using TaskManager.Core.Services;
using TaskManager.Data;
using NUnit.Framework;

namespace TaskManager.Tests;

[TestFixture]
public class TaskServiceTests
{
    private InMemoryTaskRepository _repository = null!;
    private TaskValidator _validator = null!;
    private MockNotifier _mockNotifier = null!;
    private TaskService _service = null!;

    [SetUp]
    public void SetUp()
    {
        _repository = new InMemoryTaskRepository();
        _validator = new TaskValidator();
        _mockNotifier = new MockNotifier();

        var notifiers = new Dictionary<NotificationType, ITaskNotifier>
        {
            [NotificationType.Console] = _mockNotifier
        };

        _service = new TaskService(_repository, _validator, notifiers);
    }

    [Test]
    public void Add_AndGetAll_ReturnsTask()
    {
        var task = CreateSampleTask();
        _service.Add(task);

        var all = _service.GetAll();

        Assert.That(all, Has.Count.EqualTo(1));
        Assert.That(all[0].Title, Is.EqualTo("Sample"));
    }

    [Test]
    public void GetFiltered_ByStatus_ReturnsMatchingTasks()
    {
        _service.Add(CreateSampleTask("A", TaskItemStatus.Todo));
        _service.Add(CreateSampleTask("B", TaskItemStatus.Done));

        var filtered = _service.GetFiltered(TaskItemStatus.Todo, null);

        Assert.That(filtered, Has.Count.EqualTo(1));
        Assert.That(filtered[0].Title, Is.EqualTo("A"));
    }

    [Test]
    public void Delete_RemovesTask()
    {
        var task = CreateSampleTask();
        _service.Add(task);
        var id = _service.GetAll()[0].Id;

        _service.Delete(id);

        Assert.That(_service.GetAll(), Is.Empty);
    }

    [Test]
    public void CompleteTask_InvokesInjectedNotifier_WithoutModifyingService()
    {
        var task = CreateSampleTask(notification: NotificationType.Console);
        _service.Add(task);
        var id = _service.GetAll()[0].Id;

        _service.CompleteTask(id);

        Assert.That(_mockNotifier.WasCalled, Is.True);
        Assert.That(_mockNotifier.LastTask?.Title, Is.EqualTo("Sample"));
    }

    [Test]
    public void Add_WithEmptyTitle_Throws()
    {
        var task = CreateSampleTask(title: " ");

        Assert.Throws<ArgumentException>(() => _service.Add(task));
    }

    private static TaskItem CreateSampleTask(
        string title = "Sample",
        TaskItemStatus status = TaskItemStatus.Todo,
        NotificationType notification = NotificationType.Email)
    {
        return TaskItem.Create(
            TaskTypeKind.Standard,
            title,
            "Description",
            status,
            TaskPriority.Medium,
            notification,
            null,
            null);
    }

    private sealed class MockNotifier : ITaskNotifier
    {
        public bool WasCalled { get; private set; }
        public TaskItem? LastTask { get; private set; }

        public void Notify(TaskItem task)
        {
            WasCalled = true;
            LastTask = task;
        }
    }
}
