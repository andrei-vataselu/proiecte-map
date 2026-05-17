namespace DocumentGenerator.Core.Models;

public class DocumentTemplate : ICloneable
{
    public string DefaultTitle { get; set; } = string.Empty;
    public List<DocumentSection> Sections { get; set; } = new();
    public FormattingSettings Formatting { get; set; } = new();

    public object Clone()
    {
        return new DocumentTemplate
        {
            DefaultTitle = DefaultTitle,
            Sections = Sections.Select(s => s.Clone()).ToList(),
            Formatting = Formatting.Clone()
        };
    }

    public DocumentTemplate DeepClone() => (DocumentTemplate)Clone();
}
