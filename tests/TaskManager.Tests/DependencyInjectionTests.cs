using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.Core.Interfaces;
using TaskManager.Core.Models;
using TaskManager.Core.Notifications;
using TaskManager.Core.Services;
using TaskManager.Data;
using NUnit.Framework;

namespace TaskManager.Tests;

[TestFixture]
public class DependencyInjectionTests
{
    [Test]
    public void TaskService_Constructor_DeclaresITaskRepositoryDependency()
    {
        var parameters = typeof(TaskService)
            .GetConstructors(BindingFlags.Public | BindingFlags.Instance)[0]
            .GetParameters();

        Assert.That(
            parameters.Any(p => p.ParameterType == typeof(ITaskRepository)),
            Is.True,
            "TaskService trebuie sa declare explicit ITaskRepository in constructor (DIP).");
    }

    [Test]
    public void ServiceProvider_WithoutRepositoryRegistration_CannotResolveTaskService()
    {
        var services = new ServiceCollection();
        services.AddTransient<TaskValidator>();
        services.AddSingleton<IReadOnlyDictionary<NotificationType, ITaskNotifier>>(
            _ => new Dictionary<NotificationType, ITaskNotifier>());

        var provider = services.BuildServiceProvider();

        Assert.Throws<InvalidOperationException>(() => provider.GetRequiredService<TaskService>());
    }

    [TestCase(NotificationType.Email)]
    [TestCase(NotificationType.Console)]
    [TestCase(NotificationType.FileLog)]
    [TestCase(NotificationType.Slack)]
    public void CompleteTask_InvokesMockNotifier_ForEachRegisteredType(NotificationType type)
    {
        var repository = new InMemoryTaskRepository();
        var validator = new TaskValidator();
        var mock = new TrackingNotifier();
        var notifiers = new Dictionary<NotificationType, ITaskNotifier> { [type] = mock };
        var service = new TaskService(repository, validator, notifiers);

        var task = TaskItem.Create(
            TaskTypeKind.Standard,
            "Notify",
            null,
            TaskItemStatus.Todo,
            TaskPriority.Medium,
            type,
            null,
            null);

        service.Add(task);
        service.CompleteTask(task.Id);

        Assert.That(mock.WasCalled, Is.True);
        Assert.That(mock.LastType, Is.EqualTo(type));
    }

    private sealed class TrackingNotifier : ITaskNotifier
    {
        public bool WasCalled { get; private set; }
        public NotificationType? LastType { get; private set; }

        public void Notify(TaskItem task)
        {
            WasCalled = true;
            LastType = task.NotificationType;
        }
    }
}
