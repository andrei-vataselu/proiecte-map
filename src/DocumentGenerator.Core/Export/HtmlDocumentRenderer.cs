using System.Text;
using DocumentGenerator.Core.Models;

namespace DocumentGenerator.Core.Export;

public class HtmlDocumentRenderer : IDocumentRenderer
{
    public string Render(DocumentData data)
    {
        var sb = new StringBuilder();
        sb.AppendLine("<!DOCTYPE html>");
        sb.AppendLine("<html><head><meta charset=\"utf-8\"/>");
        sb.AppendLine($"<title>{data.Title}</title></head><body>");

        if (!string.IsNullOrEmpty(data.HeaderContent))
            sb.AppendLine(data.HeaderContent);

        sb.AppendLine($"<h1>{data.Title}</h1>");
        sb.AppendLine($"<p><em>{data.Author}</em> - {data.Date:yyyy-MM-dd}</p>");

        foreach (var section in data.Sections)
        {
            sb.AppendLine($"<h2>{section.Heading}</h2>");
            sb.AppendLine($"<p>{section.Content}</p>");
        }

        if (!string.IsNullOrEmpty(data.Footnote))
            sb.AppendLine($"<p><small>{data.Footnote}</small></p>");

        if (!string.IsNullOrEmpty(data.FooterContent))
            sb.AppendLine(data.FooterContent);

        sb.AppendLine("</body></html>");
        return sb.ToString();
    }
}
