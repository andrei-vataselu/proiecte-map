using DocumentProcessor.Core.Interfaces;
using DocumentProcessor.Core.Models;

namespace DocumentProcessor.Tests.Helpers;

internal sealed class CountingFakeParser : IDocumentParser
{
    public int Calls { get; private set; }

    public Document Parse(string content)
    {
        Calls++;
        return new Document("ok", "0123456789", DocumentSourceFormat.Json);
    }
}

internal sealed class ConfigurableFakeParser : IDocumentParser
{
    private readonly Func<string, Document> _factory;

    public ConfigurableFakeParser(Func<string, Document> factory)
    {
        _factory = factory;
    }

    public Document Parse(string content) => _factory(content);
}
