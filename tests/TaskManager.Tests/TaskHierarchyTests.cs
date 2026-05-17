using TaskManager.Core.Models;
using NUnit.Framework;

namespace TaskManager.Tests;

[TestFixture]
public class TaskHierarchyTests
{
    [TestCase(typeof(TaskItem))]
    [TestCase(typeof(RecurringTask))]
    [TestCase(typeof(DeadlineTask))]
    public void Complete_SetsStatusToDone_ForAllSubtypes(Type taskType)
    {
        var task = CreateTask(taskType);
        task.Status = TaskItemStatus.Todo;

        task.Complete();

        Assert.That(task.Status, Is.EqualTo(TaskItemStatus.Done));
    }

    [Test]
    public void Complete_OnAlreadyDoneTask_Throws()
    {
        var task = new TaskItem { Title = "Done task", Status = TaskItemStatus.Done };

        Assert.Throws<InvalidOperationException>(() => task.Complete());
    }

    [Test]
    public void RecurringTask_Complete_RecalculatesDueDate()
    {
        var due = DateTime.UtcNow.AddDays(3);
        var task = new RecurringTask
        {
            Title = "Weekly",
            Status = TaskItemStatus.Todo,
            DueDate = due,
            RecurrenceInterval = 7
        };

        task.Complete();

        Assert.That(task.Status, Is.EqualTo(TaskItemStatus.Done));
        Assert.That(task.DueDate, Is.EqualTo(due.AddDays(7)));
    }

    private static TaskItem CreateTask(Type taskType) =>
        (TaskItem)Activator.CreateInstance(taskType)!;
}
