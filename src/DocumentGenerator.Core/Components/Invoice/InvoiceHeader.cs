using DocumentGenerator.Core.Models;

namespace DocumentGenerator.Core.Components.Invoice;

public class InvoiceHeader : IDocumentHeader
{
    public string Render(DocumentData data) =>
        $"[FACTURA] CUI: RO12345678 | Nr. factura: INV-{data.Date:yyyyMMdd} | Client: {data.Title}";
}
