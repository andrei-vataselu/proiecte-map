using DocumentGenerator.Core.Assembly;
using DocumentGenerator.Core.Builder;
using DocumentGenerator.Core.Components.Invoice;
using DocumentGenerator.Core.Components.Report;
using NUnit.Framework;

namespace DocumentGenerator.Tests;

[TestFixture]
public class AbstractFactoryTests
{
    [Test]
    public void DocumentAssembler_WithReportFactory_ProducesReportHeader()
    {
        var data = CreateSampleData();
        var assembler = new DocumentAssembler(new ReportComponentFactory());

        var result = assembler.Assemble(data);

        Assert.That(result.HeaderContent, Does.Contain("[RAPORT]"));
        Assert.That(result.HeaderContent, Does.Contain("Logo"));
    }

    [Test]
    public void DocumentAssembler_WithInvoiceFactory_ProducesInvoiceHeader()
    {
        var data = CreateSampleData();
        var assembler = new DocumentAssembler(new InvoiceComponentFactory());

        var result = assembler.Assemble(data);

        Assert.That(result.HeaderContent, Does.Contain("[FACTURA]"));
        Assert.That(result.HeaderContent, Does.Contain("CUI"));
    }

    private static Core.Models.DocumentData CreateSampleData() =>
        new DocumentDataBuilder()
            .WithTitle("Document test")
            .ByAuthor("Autor")
            .WithSection("Linie 1", "Valoare 100")
            .Build();
}
