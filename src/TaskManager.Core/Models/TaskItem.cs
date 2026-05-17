namespace TaskManager.Core.Models;

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TaskItemStatus Status { get; set; } = TaskItemStatus.Todo;
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;
    public TaskTypeKind TaskType { get; set; } = TaskTypeKind.Standard;
    public NotificationType NotificationType { get; set; } = NotificationType.Console;
    public DateTime? DueDate { get; set; }
    public int? RecurrenceInterval { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public bool IsOverdue =>
        Status != TaskItemStatus.Done
        && DueDate.HasValue
        && DueDate.Value < DateTime.UtcNow;

    public void Complete()
    {
        if (Status == TaskItemStatus.Done)
            throw new InvalidOperationException("Task is already Done.");

        CompleteCore();

        if (Status != TaskItemStatus.Done)
            throw new InvalidOperationException("Postcondition failed: Status must be Done after Complete().");

        AssertInvariant();
    }

    protected virtual void CompleteCore()
    {
        Status = TaskItemStatus.Done;
    }

    protected void AssertInvariant()
    {
        if (Status == TaskItemStatus.Done && IsOverdue)
            throw new InvalidOperationException("Invariant violated: a task cannot be Done and Overdue at the same time.");
    }

    public static TaskItem Create(
        TaskTypeKind taskType,
        string title,
        string? description,
        TaskItemStatus status,
        TaskPriority priority,
        NotificationType notificationType,
        DateTime? dueDate,
        int? recurrenceInterval)
    {
        TaskItem task = taskType switch
        {
            TaskTypeKind.Recurring => new RecurringTask(),
            TaskTypeKind.Deadline => new DeadlineTask(),
            _ => new TaskItem()
        };

        task.Title = title;
        task.Description = description;
        task.Status = status;
        task.Priority = priority;
        task.TaskType = taskType;
        task.NotificationType = notificationType;
        task.DueDate = dueDate;
        task.RecurrenceInterval = recurrenceInterval;
        task.CreatedAt = DateTime.UtcNow;

        return task;
    }
}
