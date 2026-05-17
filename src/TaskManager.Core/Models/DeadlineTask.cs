namespace TaskManager.Core.Models;

public class DeadlineTask : TaskItem
{
    protected override void CompleteCore()
    {
        Status = TaskItemStatus.Done;
    }
}
