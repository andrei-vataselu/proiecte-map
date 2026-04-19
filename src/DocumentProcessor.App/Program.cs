using DocumentProcessor.Core.Adapter;
using DocumentProcessor.Core.Decorators;
using DocumentProcessor.Core.Facade;

var root = Directory.GetCurrentDirectory();
var samplesDir = Path.Combine(root, "samples");
Directory.CreateDirectory(samplesDir);

var xmlPath = Path.Combine(samplesDir, "demo.xml");
var jsonPath = Path.Combine(samplesDir, "demo.json");

File.WriteAllText(xmlPath,
    "<legacyDocument><header>Demo</header><body>0123456789</body></legacyDocument>");
File.WriteAllText(jsonPath, "{\"title\":\"Demo\",\"content\":\"0123456789\"}");

var outputDir = Path.Combine(root, "output");
var facade = new DocumentProcessingFacade(outputDir);

foreach (var path in new[] { xmlPath, jsonPath })
{
    var result = facade.Process(path);
    Console.WriteLine(
        $"{Path.GetFileName(path)} ok={result.IsSuccess} msg={result.Message} saved={result.OutputPath}");
}

var stacked = new LoggingDocumentParser(
    new ValidationDocumentParser(
        new XmlParserAdapter(new LegacyXmlParser())));
var stackedDoc = stacked.Parse(File.ReadAllText(xmlPath));
Console.WriteLine($"stacked parse title={stackedDoc.Title}");
