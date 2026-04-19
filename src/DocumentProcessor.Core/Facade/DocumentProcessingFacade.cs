using DocumentProcessor.Core.Adapter;
using DocumentProcessor.Core.Decorators;
using DocumentProcessor.Core.Exceptions;
using DocumentProcessor.Core.Interfaces;
using DocumentProcessor.Core.Models;
using DocumentProcessor.Core.Parsers;
using DocumentProcessor.Core.Storage;

namespace DocumentProcessor.Core.Facade;

public sealed class DocumentProcessingFacade
{
    private readonly string _outputDirectory;
    private readonly DocumentSaver _saver;

    public DocumentProcessingFacade(string outputDirectory)
    {
        _outputDirectory = outputDirectory;
        _saver = new DocumentSaver();
    }

    public ProcessingResult Process(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                return ProcessingResult.Fail("File not found");
            }

            var extension = Path.GetExtension(filePath).ToLowerInvariant();
            IDocumentParser? core = extension switch
            {
                ".xml" => new XmlParserAdapter(new LegacyXmlParser()),
                ".json" => new JsonDocumentParser(),
                _ => null
            };

            if (core is null)
            {
                return ProcessingResult.Fail("Unsupported file extension");
            }

            var parser = new LoggingDocumentParser(
                new ValidationDocumentParser(
                    new CachingDocumentParser(core)));

            var fileContent = File.ReadAllText(filePath);
            var document = parser.Parse(fileContent);
            var internalText = DocumentTransformer.ToInternalText(document);
            var flavor = extension == ".json" ? "json" : "xml";
            var outputName = $"{Path.GetFileNameWithoutExtension(filePath)}_{flavor}.txt";
            var outputPath = Path.Combine(_outputDirectory, outputName);
            _saver.Save(outputPath, internalText);
            return ProcessingResult.Ok(outputPath, "Processed");
        }
        catch (ValidationException ex)
        {
            return ProcessingResult.Fail(ex.Message);
        }
        catch (Exception ex)
        {
            return ProcessingResult.Fail(ex.Message);
        }
    }
}
