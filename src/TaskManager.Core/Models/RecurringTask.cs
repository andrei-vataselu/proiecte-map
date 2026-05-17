namespace TaskManager.Core.Models;

public class RecurringTask : TaskItem
{
    protected override void CompleteCore()
    {
        Status = TaskItemStatus.Done;

        if (DueDate.HasValue && RecurrenceInterval.HasValue)
            DueDate = DueDate.Value.AddDays(RecurrenceInterval.Value);
    }
}
