using DocumentGenerator.Core.Models;

namespace DocumentGenerator.Core.Export;

public interface IDocumentRenderer
{
    string Render(DocumentData data);
}
