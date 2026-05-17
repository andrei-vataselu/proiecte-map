using DocumentGenerator.Core.Components;

namespace DocumentGenerator.Core.Components.Invoice;

public class InvoiceComponentFactory : IDocumentComponentFactory
{
    public IDocumentHeader CreateHeader() => new InvoiceHeader();
    public IDocumentSectionComponent CreateSection() => new InvoiceSectionComponent();
    public IDocumentFooter CreateFooter() => new InvoiceFooter();
}
