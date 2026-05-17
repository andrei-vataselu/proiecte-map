namespace DocumentGenerator.Core.Components;

public interface IDocumentComponentFactory
{
    IDocumentHeader CreateHeader();
    IDocumentSectionComponent CreateSection();
    IDocumentFooter CreateFooter();
}
