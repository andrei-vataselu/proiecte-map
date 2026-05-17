using DocumentGenerator.Core.Models;

namespace DocumentGenerator.Core.Components;

public interface IDocumentSectionComponent
{
    string Render(DocumentSection section, DocumentData data);
}
