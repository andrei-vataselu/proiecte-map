namespace DocumentGenerator.Core.Export;

public class HtmlDocumentExporter : DocumentExporter
{
    protected override IDocumentRenderer CreateRenderer() => new HtmlDocumentRenderer();
}
