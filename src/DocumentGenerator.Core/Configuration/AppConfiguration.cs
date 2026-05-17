using System.Text.Json;
using DocumentGenerator.Core.Models;

namespace DocumentGenerator.Core.Configuration;

public sealed class AppConfiguration
{
    private static readonly Lazy<AppConfiguration> LazyInstance = new(LoadConfiguration);

    public static AppConfiguration Instance => LazyInstance.Value;

    public string OutputDirectory { get; private set; } = "output";
    public string DefaultFormat { get; private set; } = "html";
    public string DefaultAuthor { get; private set; } = "Document Generator";

    private AppConfiguration()
    {
    }

    private static AppConfiguration LoadConfiguration()
    {
        var config = new AppConfiguration();
        var path = Path.Combine(AppContext.BaseDirectory, "appsettings.json");

        if (!File.Exists(path))
            return config;

        try
        {
            var json = File.ReadAllText(path);
            using var document = JsonDocument.Parse(json);
            var root = document.RootElement;

            if (root.TryGetProperty("OutputDirectory", out var outputDir))
                config.OutputDirectory = outputDir.GetString() ?? config.OutputDirectory;

            if (root.TryGetProperty("DefaultFormat", out var format))
                config.DefaultFormat = format.GetString() ?? config.DefaultFormat;

            if (root.TryGetProperty("DefaultAuthor", out var author))
                config.DefaultAuthor = author.GetString() ?? config.DefaultAuthor;
        }
        catch
        {
            return config;
        }

        return config;
    }
}
