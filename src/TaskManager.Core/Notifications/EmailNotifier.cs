using TaskManager.Core.Interfaces;
using TaskManager.Core.Models;

namespace TaskManager.Core.Notifications;

public class EmailNotifier : ITaskNotifier
{
    public void Notify(TaskItem task)
    {
        Console.WriteLine($"Email sent to team@example.com — Task completed: \"{task.Title}\" (Id: {task.Id})");
    }
}
