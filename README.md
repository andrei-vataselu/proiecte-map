# DrawingTool

DrawingTool este o aplicatie .NET care foloseste:

- Composite pentru ierarhia de forme (`Picture` poate contine alte `Picture`)
- Bridge pentru separarea modelului de randare (`IShape` deseneaza prin `ICanvas`)
- Proxy pentru blocarea operatiilor de modificare (`ReadOnlyShapeProxy`)

## Structura

- `src/DrawingTool.Core` - modelul si pattern-urile
- `src/DrawingTool.Wpf` - interfata WPF pentru demo vizual si export SVG
- `tests/DrawingTool.Tests` - teste NUnit

## Exemplu scena imbricata

```csharp
var root = new Picture();
root.Add(new Line(30, 30, 160, 110));
root.Add(new Rectangle(180, 40, 160, 90));

var nested = new Picture();
nested.Add(new Circle(130, 220, 40));
nested.Add(new Ellipse(270, 230, 70, 35));

var deepNested = new Picture();
deepNested.Add(new Rectangle(360, 190, 120, 80));
deepNested.Add(new Line(350, 280, 500, 340));

nested.Add(deepNested);
root.Add(nested);
```

## Exemplu SVG generat

```xml
<?xml version="1.0" encoding="UTF-8"?>
<svg xmlns="http://www.w3.org/2000/svg" version="1.1">
  <line x1="30" y1="30" x2="160" y2="110" stroke="black" fill="none" />
  <rect x="180" y="40" width="160" height="90" stroke="black" fill="none" />
  <circle cx="130" cy="220" r="40" stroke="black" fill="none" />
  <ellipse cx="270" cy="230" rx="70" ry="35" stroke="black" fill="none" />
</svg>
```

## Rulare

- Build: `dotnet build DrawingTool.sln`
- Teste: `dotnet test DrawingTool.sln`
- UI WPF: `dotnet run --project src/DrawingTool.Wpf`
