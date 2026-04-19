namespace DocumentProcessor.Core.Models;

public enum DocumentSourceFormat
{
    XmlLegacy,
    Json
}

public sealed record Document(string Title, string Content, DocumentSourceFormat SourceFormat);
