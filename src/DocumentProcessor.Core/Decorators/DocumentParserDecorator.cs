using DocumentProcessor.Core.Interfaces;
using DocumentProcessor.Core.Models;

namespace DocumentProcessor.Core.Decorators;

public abstract class DocumentParserDecorator : IDocumentParser
{
    private readonly IDocumentParser _inner;

    protected DocumentParserDecorator(IDocumentParser inner)
    {
        _inner = inner;
    }

    protected IDocumentParser Inner => _inner;

    public virtual Document Parse(string content) => _inner.Parse(content);
}
