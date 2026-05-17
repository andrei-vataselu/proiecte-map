using DocumentGenerator.Core.Models;

namespace DocumentGenerator.Core.Components.Report;

public class ReportSectionComponent : IDocumentSectionComponent
{
    public string Render(DocumentSection section, DocumentData data) =>
        $"[TABEL DATE] {section.Heading}: {section.Content}";
}
