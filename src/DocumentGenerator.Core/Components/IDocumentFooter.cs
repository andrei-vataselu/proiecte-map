using DocumentGenerator.Core.Models;

namespace DocumentGenerator.Core.Components;

public interface IDocumentFooter
{
    string Render(DocumentData data);
}
