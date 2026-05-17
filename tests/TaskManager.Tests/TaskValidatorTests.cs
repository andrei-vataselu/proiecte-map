using TaskManager.Core.Models;
using TaskManager.Core.Services;
using NUnit.Framework;

namespace TaskManager.Tests;

[TestFixture]
public class TaskValidatorTests
{
    private TaskValidator _validator = null!;

    [SetUp]
    public void SetUp() => _validator = new TaskValidator();

    [Test]
    public void Validate_RejectsEmptyTitle()
    {
        var task = new TaskItem { Title = "   " };

        var errors = _validator.Validate(task);

        Assert.That(errors, Has.Some.Contains("Title"));
    }

    [Test]
    public void Validate_RejectsTitleLongerThan200Characters()
    {
        var task = new TaskItem { Title = new string('x', 201) };

        var errors = _validator.Validate(task);

        Assert.That(errors, Has.Some.Contains("200"));
    }

    [Test]
    public void Validate_RejectsDeadlineTaskWithPastDueDate()
    {
        var task = TaskItem.Create(
            TaskTypeKind.Deadline,
            "Deadline",
            null,
            TaskItemStatus.Todo,
            TaskPriority.High,
            NotificationType.Console,
            DateTime.UtcNow.AddDays(-1),
            null);

        var errors = _validator.Validate(task);

        Assert.That(errors, Has.Some.Contains("future"));
    }
}
