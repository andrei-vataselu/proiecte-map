using TaskManager.Core.Interfaces;
using TaskManager.Core.Models;
using TaskManager.Core.Services;
using TaskManager.Data;
using NUnit.Framework;

namespace TaskManager.Tests;

[TestFixture]
public class ReportServiceTests
{
    [Test]
    public void Constructor_AcceptsInMemoryRepository_AsITaskReader()
    {
        ITaskReader reader = new InMemoryTaskRepository();
        Assert.DoesNotThrow(() => _ = new ReportService(reader));
    }

    [Test]
    public void GenerateSummary_ReturnsCorrectCounts_ForMixedStatuses()
    {
        var repository = new InMemoryTaskRepository();
        var reportService = new ReportService(repository);

        repository.Add(CreateTask("A", TaskItemStatus.Todo));
        repository.Add(CreateTask("B", TaskItemStatus.Done));
        repository.Add(CreateTask("C", TaskItemStatus.Done));
        repository.Add(CreateTask("D", TaskItemStatus.InProgress));

        var summary = reportService.GenerateSummary();

        Assert.That(summary, Does.Contain("Total sarcini: 4"));
        Assert.That(summary, Does.Contain("finalizate (Done): 2"));
    }

    private static TaskItem CreateTask(string title, TaskItemStatus status) =>
        TaskItem.Create(
            TaskTypeKind.Standard,
            title,
            null,
            status,
            TaskPriority.Medium,
            NotificationType.Console,
            null,
            null);
}
