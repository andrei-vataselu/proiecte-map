using TaskManager.Core.Interfaces;
using TaskManager.Core.Models;

namespace TaskManager.Core.Notifications;

public class FileLogNotifier : ITaskNotifier
{
    private readonly string _logPath;

    public FileLogNotifier(string? logPath = null)
    {
        _logPath = logPath ?? Path.Combine(AppContext.BaseDirectory, "tasks.log");
    }

    public void Notify(TaskItem task)
    {
        var line = $"{DateTime.UtcNow:O} | Task {task.Id} completed: {task.Title}{Environment.NewLine}";
        File.AppendAllText(_logPath, line);
    }
}
