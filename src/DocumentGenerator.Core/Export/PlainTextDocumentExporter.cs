namespace DocumentGenerator.Core.Export;

public class PlainTextDocumentExporter : DocumentExporter
{
    protected override IDocumentRenderer CreateRenderer() => new PlainTextDocumentRenderer();
}
