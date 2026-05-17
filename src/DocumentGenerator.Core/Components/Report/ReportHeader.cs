using DocumentGenerator.Core.Models;

namespace DocumentGenerator.Core.Components.Report;

public class ReportHeader : IDocumentHeader
{
    public string Render(DocumentData data) =>
        $"[RAPORT] Logo | Nr. raport: RPT-{data.Date:yyyyMMdd} | {data.Title}";
}
