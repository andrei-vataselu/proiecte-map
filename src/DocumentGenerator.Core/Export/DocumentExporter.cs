using DocumentGenerator.Core.Models;

namespace DocumentGenerator.Core.Export;

public abstract class DocumentExporter
{
    public string Export(DocumentData data)
    {
        var renderer = CreateRenderer();
        return renderer.Render(data);
    }

    protected abstract IDocumentRenderer CreateRenderer();
}
