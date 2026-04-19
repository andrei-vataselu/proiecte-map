using DocumentProcessor.Core.Models;

namespace DocumentProcessor.Core.Storage;

public static class DocumentTransformer
{
    public static string ToInternalText(Document document)
    {
        return $"TITLE: {document.Title}{Environment.NewLine}" +
               $"CONTENT:{Environment.NewLine}{document.Content}{Environment.NewLine}" +
               $"SOURCE: {document.SourceFormat}{Environment.NewLine}";
    }
}
