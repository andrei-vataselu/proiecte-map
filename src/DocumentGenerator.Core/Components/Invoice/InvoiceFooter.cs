using DocumentGenerator.Core.Models;

namespace DocumentGenerator.Core.Components.Invoice;

public class InvoiceFooter : IDocumentFooter
{
    public string Render(DocumentData data)
    {
        var total = data.Sections.Count * 100;
        return $"[SUBSOL FACTURA] TOTAL DE PLATA: {total} RON | Termen plata: 30 zile";
    }
}
