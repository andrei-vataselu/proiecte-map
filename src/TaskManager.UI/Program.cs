using Microsoft.Extensions.DependencyInjection;
using TaskManager.Core.Interfaces;
using TaskManager.Core.Services;
using TaskManager.Data;

namespace TaskManager.UI;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();

        var services = new ServiceCollection();

        services.AddSingleton<SqliteTaskRepository>();
        services.AddSingleton<ITaskRepository>(sp => sp.GetRequiredService<SqliteTaskRepository>());
        services.AddSingleton<ITaskReader>(sp => sp.GetRequiredService<SqliteTaskRepository>());
        services.AddTaskManagerNotifiers();
        services.AddTransient<TaskValidator>();
        services.AddTransient<TaskService>();
        services.AddTransient<ReportService>();

        var provider = services.BuildServiceProvider();

        Application.Run(new MainForm(
            provider.GetRequiredService<TaskService>(),
            provider.GetRequiredService<ReportService>()));
    }
}
