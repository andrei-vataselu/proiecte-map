using DocumentProcessor.Core.Models;

namespace DocumentProcessor.Core.Interfaces;

public interface IDocumentParser
{
    Document Parse(string content);
}
