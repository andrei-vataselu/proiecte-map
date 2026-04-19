namespace DocumentProcessor.Core.Models;

public sealed class ProcessingResult
{
    public bool IsSuccess { get; init; }
    public string Message { get; init; } = string.Empty;
    public string? OutputPath { get; init; }

    public static ProcessingResult Ok(string outputPath, string message) =>
        new()
        {
            IsSuccess = true,
            Message = message,
            OutputPath = outputPath
        };

    public static ProcessingResult Fail(string message) =>
        new()
        {
            IsSuccess = false,
            Message = message,
            OutputPath = null
        };
}
