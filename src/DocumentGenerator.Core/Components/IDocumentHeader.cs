using DocumentGenerator.Core.Models;

namespace DocumentGenerator.Core.Components;

public interface IDocumentHeader
{
    string Render(DocumentData data);
}
