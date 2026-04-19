using DocumentProcessor.Core.Facade;
using DocumentProcessor.Core.Models;
using NUnit.Framework;

namespace DocumentProcessor.Tests;

public sealed class FacadeTests
{
    [Test]
    public void Process_InvalidDocument_ReturnsFailureWithoutThrowing()
    {
        var workspace = Path.Combine(Path.GetTempPath(), "dp_facade_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(workspace);
        var file = Path.Combine(workspace, "bad.json");
        File.WriteAllText(file, "{\"title\":\"\",\"content\":\"1234567890\"}");
        var outputDir = Path.Combine(workspace, "out");
        var facade = new DocumentProcessingFacade(outputDir);

        ProcessingResult result = facade.Process(file);

        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Message, Is.Not.Empty);
    }
}
