namespace DocumentProcessor.Core.Storage;

public sealed class DocumentSaver
{
    public string Save(string fullPath, string text)
    {
        var directory = Path.GetDirectoryName(fullPath);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        File.WriteAllText(fullPath, text);
        return fullPath;
    }
}
