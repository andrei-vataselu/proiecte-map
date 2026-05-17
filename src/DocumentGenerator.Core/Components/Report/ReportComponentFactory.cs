using DocumentGenerator.Core.Components;

namespace DocumentGenerator.Core.Components.Report;

public class ReportComponentFactory : IDocumentComponentFactory
{
    public IDocumentHeader CreateHeader() => new ReportHeader();
    public IDocumentSectionComponent CreateSection() => new ReportSectionComponent();
    public IDocumentFooter CreateFooter() => new ReportFooter();
}
