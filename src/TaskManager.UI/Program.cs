using TaskManager.Core.Interfaces;
using TaskManager.Core.Models;
using TaskManager.Core.Notifications;
using TaskManager.Core.Services;
using TaskManager.Data;

namespace TaskManager.UI;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();

        ITaskRepository repository = new SqliteTaskRepository();
        var validator = new TaskValidator();

        IReadOnlyDictionary<NotificationType, ITaskNotifier> notifiers =
            new Dictionary<NotificationType, ITaskNotifier>
            {
                [NotificationType.Email] = new EmailNotifier(),
                [NotificationType.Console] = new ConsoleNotifier(),
                [NotificationType.FileLog] = new FileLogNotifier(),
                [NotificationType.Slack] = new SlackNotifier()
            };

        var service = new TaskService(repository, validator, notifiers);
        Application.Run(new MainForm(service));
    }
}
