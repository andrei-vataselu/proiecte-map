using DocumentGenerator.Core.Models;

namespace DocumentGenerator.Core.Components.Report;

public class ReportFooter : IDocumentFooter
{
    public string Render(DocumentData data) =>
        $"[SUBSOL RAPORT] Semnatura: ____________ | Intocmit de {data.Author}";
}
