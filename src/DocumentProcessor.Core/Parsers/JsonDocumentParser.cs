using System.Text.Json;
using DocumentProcessor.Core.Interfaces;
using DocumentProcessor.Core.Models;

namespace DocumentProcessor.Core.Parsers;

public sealed class JsonDocumentParser : IDocumentParser
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public Document Parse(string content)
    {
        var dto = JsonSerializer.Deserialize<JsonPayload>(content, Options)
                  ?? throw new InvalidOperationException("JSON payload is null");
        return new Document(dto.Title ?? string.Empty, dto.Content ?? string.Empty, DocumentSourceFormat.Json);
    }

    private sealed class JsonPayload
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
    }
}
