using DocumentGenerator.Core.Assembly;
using DocumentGenerator.Core.Builder;
using DocumentGenerator.Core.Components;
using DocumentGenerator.Core.Components.Invoice;
using DocumentGenerator.Core.Components.Report;
using DocumentGenerator.Core.Configuration;
using DocumentGenerator.Core.Export;
using DocumentGenerator.Core.Prototype;

var config = AppConfiguration.Instance;
var registry = new TemplateRegistry();
DefaultTemplates.RegisterAll(registry);

DocumentExporter exporter = config.DefaultFormat.Equals("txt", StringComparison.OrdinalIgnoreCase)
    ? new PlainTextDocumentExporter()
    : new HtmlDocumentExporter();

Console.WriteLine("=== Document Generator - Laborator 5 ===");
Console.WriteLine($"Autor implicit: {config.DefaultAuthor}");
Console.WriteLine($"Director iesire: {config.OutputDirectory}");
Console.WriteLine();

GenerateDocument(
    "report",
    new ReportComponentFactory(),
    exporter,
    registry,
    config);

GenerateDocument(
    "invoice",
    new InvoiceComponentFactory(),
    new HtmlDocumentExporter(),
    registry,
    config);

Console.WriteLine("Documente generate cu succes.");

static void GenerateDocument(
    string templateKey,
    IDocumentComponentFactory componentFactory,
    DocumentExporter exporter,
    TemplateRegistry registry,
    AppConfiguration config)
{
    var template = registry.GetClone(templateKey);

    var data = new DocumentDataBuilder()
        .WithTitle(template.DefaultTitle)
        .ByAuthor(config.DefaultAuthor)
        .WithDate(DateTime.UtcNow);

    foreach (var section in template.Sections)
        data.WithSection(section.Heading, section.Content);

    var document = data.Build();
    document.PageFormat = template.Formatting.PageFormat;
    document.Orientation = template.Formatting.Orientation;

    var assembler = new DocumentAssembler(componentFactory);
    assembler.Assemble(document);

    var output = exporter.Export(document);

    Directory.CreateDirectory(config.OutputDirectory);
    var extension = exporter is HtmlDocumentExporter ? "html" : "txt";
    var fileName = Path.Combine(config.OutputDirectory, $"{templateKey}-{DateTime.UtcNow:yyyyMMddHHmmss}.{extension}");
    File.WriteAllText(fileName, output);

    Console.WriteLine($"Salvat: {fileName}");
}
