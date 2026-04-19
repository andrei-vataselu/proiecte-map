using DocumentProcessor.Core.Interfaces;
using DocumentProcessor.Core.Models;

namespace DocumentProcessor.Core.Decorators;

public sealed class LoggingDocumentParser : DocumentParserDecorator
{
    public LoggingDocumentParser(IDocumentParser inner) : base(inner)
    {
    }

    public override Document Parse(string content)
    {
        var start = DateTime.UtcNow;
        Console.WriteLine($"parse start {start:O}");
        try
        {
            var document = Inner.Parse(content);
            var end = DateTime.UtcNow;
            var size = (document.Title?.Length ?? 0) + (document.Content?.Length ?? 0);
            Console.WriteLine($"parse end {end:O} size={size}");
            return document;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"parse failed {ex.GetType().Name}: {ex.Message}");
            throw;
        }
    }
}
