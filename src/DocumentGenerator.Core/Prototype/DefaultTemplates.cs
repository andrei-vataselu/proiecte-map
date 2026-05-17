using DocumentGenerator.Core.Models;

namespace DocumentGenerator.Core.Prototype;

public static class DefaultTemplates
{
    public static void RegisterAll(TemplateRegistry registry)
    {
        registry.Register("report", new DocumentTemplate
        {
            DefaultTitle = "Raport activitate",
            Sections =
            {
                new DocumentSection { Heading = "Rezumat", Content = "Date sumare..." },
                new DocumentSection { Heading = "Detalii", Content = "Informatii detaliate..." }
            },
            Formatting = new FormattingSettings { PageFormat = PageFormat.A4, Orientation = PageOrientation.Portrait }
        });

        registry.Register("invoice", new DocumentTemplate
        {
            DefaultTitle = "Factura servicii",
            Sections =
            {
                new DocumentSection { Heading = "Servicii", Content = "Consultanta - 10 ore" },
                new DocumentSection { Heading = "Materiale", Content = "Licente software" }
            },
            Formatting = new FormattingSettings { PageFormat = PageFormat.A4, Orientation = PageOrientation.Portrait }
        });
    }
}
