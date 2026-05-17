using TaskManager.Core.Interfaces;
using TaskManager.Core.Models;

namespace TaskManager.Core.Notifications;

public class SlackNotifier : ITaskNotifier
{
    public void Notify(TaskItem task)
    {
        Console.WriteLine($"[Slack] #tasks channel - \"{task.Title}\" completed (Id: {task.Id})");
    }
}
