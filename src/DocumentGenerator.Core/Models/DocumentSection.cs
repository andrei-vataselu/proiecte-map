namespace DocumentGenerator.Core.Models;

public class DocumentSection
{
    public string Heading { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;

    public DocumentSection Clone() =>
        new() { Heading = Heading, Content = Content };
}
