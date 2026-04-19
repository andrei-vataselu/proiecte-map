using DocumentProcessor.Core.Exceptions;
using DocumentProcessor.Core.Interfaces;
using DocumentProcessor.Core.Models;

namespace DocumentProcessor.Core.Decorators;

public sealed class ValidationDocumentParser : DocumentParserDecorator
{
    public ValidationDocumentParser(IDocumentParser inner) : base(inner)
    {
    }

    public override Document Parse(string content)
    {
        var document = Inner.Parse(content);
        if (string.IsNullOrWhiteSpace(document.Title))
        {
            throw new ValidationException("Title is required");
        }

        if (string.IsNullOrWhiteSpace(document.Content))
        {
            throw new ValidationException("Content is required");
        }

        if (document.Content.Trim().Length < 10)
        {
            throw new ValidationException("Content must be at least 10 characters");
        }

        return document;
    }
}
