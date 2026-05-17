using Microsoft.Extensions.DependencyInjection;
using TaskManager.Core.Interfaces;
using TaskManager.Core.Models;
using TaskManager.Core.Notifications;

namespace TaskManager.UI;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTaskManagerNotifiers(this IServiceCollection services)
    {
        services.AddSingleton<IReadOnlyDictionary<NotificationType, ITaskNotifier>>(_ =>
            new Dictionary<NotificationType, ITaskNotifier>
            {
                [NotificationType.Email] = new EmailNotifier(),
                [NotificationType.Console] = new ConsoleNotifier(),
                [NotificationType.FileLog] = new FileLogNotifier(),
                [NotificationType.Slack] = new SlackNotifier()
            });

        return services;
    }
}
