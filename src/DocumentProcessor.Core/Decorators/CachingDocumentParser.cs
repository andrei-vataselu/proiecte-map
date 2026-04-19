using DocumentProcessor.Core.Interfaces;
using DocumentProcessor.Core.Models;
using DocumentProcessor.Core.Utilities;

namespace DocumentProcessor.Core.Decorators;

public sealed class CachingDocumentParser : DocumentParserDecorator
{
    private readonly Dictionary<string, Document> _cache = new();

    public CachingDocumentParser(IDocumentParser inner) : base(inner)
    {
    }

    public override Document Parse(string content)
    {
        var key = ContentHasher.ComputeSha256Hex(content);
        if (_cache.TryGetValue(key, out var cached))
        {
            return cached;
        }

        var document = Inner.Parse(content);
        _cache[key] = document;
        return document;
    }
}
