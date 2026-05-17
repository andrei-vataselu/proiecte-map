using System.Text;
using DocumentGenerator.Core.Models;

namespace DocumentGenerator.Core.Export;

public class PlainTextDocumentRenderer : IDocumentRenderer
{
    public string Render(DocumentData data)
    {
        var sb = new StringBuilder();

        if (!string.IsNullOrEmpty(data.HeaderContent))
            sb.AppendLine(data.HeaderContent);

        sb.AppendLine(data.Title.ToUpperInvariant());
        sb.AppendLine(new string('=', data.Title.Length));
        sb.AppendLine($"{data.Author} | {data.Date:yyyy-MM-dd}");
        sb.AppendLine();

        foreach (var section in data.Sections)
        {
            sb.AppendLine(section.Heading);
            sb.AppendLine(new string('-', section.Heading.Length));
            sb.AppendLine(section.Content);
            sb.AppendLine();
        }

        if (!string.IsNullOrEmpty(data.Footnote))
            sb.AppendLine($"Nota: {data.Footnote}");

        if (!string.IsNullOrEmpty(data.FooterContent))
            sb.AppendLine(data.FooterContent);

        return sb.ToString();
    }
}
