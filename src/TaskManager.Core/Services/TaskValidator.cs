using TaskManager.Core.Models;

namespace TaskManager.Core.Services;

public class TaskValidator
{
    public IReadOnlyList<string> Validate(TaskItem task)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(task.Title))
            errors.Add("Title is required.");

        if (task.Title.Length > 200)
            errors.Add("Title must be at most 200 characters.");

        if (task.TaskType == TaskTypeKind.Deadline)
        {
            if (!task.DueDate.HasValue)
                errors.Add("DueDate is required for Deadline tasks.");
            else if (task.DueDate.Value <= DateTime.UtcNow)
                errors.Add("DueDate must be in the future for Deadline tasks.");
        }

        if (task.TaskType == TaskTypeKind.Recurring && (!task.RecurrenceInterval.HasValue || task.RecurrenceInterval <= 0))
            errors.Add("RecurrenceInterval must be a positive number of days for Recurring tasks.");

        return errors;
    }

    public void ValidateOrThrow(TaskItem task)
    {
        var errors = Validate(task);
        if (errors.Count > 0)
            throw new ArgumentException(string.Join(" ", errors));
    }
}
