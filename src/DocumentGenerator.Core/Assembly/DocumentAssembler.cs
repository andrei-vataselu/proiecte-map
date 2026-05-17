using DocumentGenerator.Core.Components;
using DocumentGenerator.Core.Models;

namespace DocumentGenerator.Core.Assembly;

public class DocumentAssembler
{
    private readonly IDocumentComponentFactory _componentFactory;

    public DocumentAssembler(IDocumentComponentFactory componentFactory)
    {
        _componentFactory = componentFactory;
    }

    public DocumentData Assemble(DocumentData data)
    {
        var header = _componentFactory.CreateHeader();
        var sectionComponent = _componentFactory.CreateSection();
        var footer = _componentFactory.CreateFooter();

        data.HeaderContent = header.Render(data);

        for (var i = 0; i < data.Sections.Count; i++)
        {
            var section = data.Sections[i];
            data.Sections[i] = new DocumentSection
            {
                Heading = section.Heading,
                Content = sectionComponent.Render(section, data)
            };
        }

        data.FooterContent = footer.Render(data);
        return data;
    }
}
