using DocumentGenerator.Core.Models;

namespace DocumentGenerator.Core.Components.Invoice;

public class InvoiceSectionComponent : IDocumentSectionComponent
{
    public string Render(DocumentSection section, DocumentData data) =>
        $"[LINIE FACTURA] {section.Heading} | Cantitate/Valoare: {section.Content}";
}
