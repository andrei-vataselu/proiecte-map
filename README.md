# DocumentGenerator — Laborator 5 (Pattern-uri creationale)

Sistem de generare documente (rapoarte si facturi) in formate HTML si Plain Text.

## Rulare

```bash
dotnet restore
dotnet test
dotnet run --project src/DocumentGenerator.App/DocumentGenerator.App.csproj
```

Documentele generate apar in folderul `output/` (configurabil in `appsettings.json`).

## Structura

```
DocumentGenerator.sln
├── src/DocumentGenerator.Core/   — modele, pattern-uri
├── src/DocumentGenerator.App/    — aplicatie consola (client)
└── tests/DocumentGenerator.Tests/
```

## Pattern-uri aplicate

### Factory Method

**Problema:** clientul nu trebuie sa cunoasca renderer-ul concret (HTML vs text).

**Unde:** `DocumentExporter` (abstract) + `CreateRenderer()`; `HtmlDocumentExporter`, `PlainTextDocumentExporter`; `IDocumentRenderer.Render()`.

Clientul primeste `DocumentExporter` injectat si apeleaza `Export()` fara `new` pe renderere.

### Abstract Factory

**Problema:** rapoartele si facturile au familii diferite de componente (antet, sectiune, subsol) care trebuie sa ramana compatibile.

**Unde:** `IDocumentComponentFactory`, `ReportComponentFactory`, `InvoiceComponentFactory`, `DocumentAssembler`.

Assembler-ul nu stie daca asambleaza raport sau factura — primeste doar factory-ul.

### Builder

**Problema:** `DocumentData` are multi parametri optionali; constructia pas cu pas cu validare centralizata.

**Unde:** `DocumentDataBuilder` — `WithTitle()`, `ByAuthor()`, `WithSection()`, `InLandscape()`, `WithFootnote()`, `Build()`.

`Build()` valideaza titlu, autor si minim o sectiune.

### Prototype

**Problema:** crearea documentelor de la zero e costisitoare; sabloanele se cloneaza si se personalizeaza.

**Unde:** `DocumentTemplate.DeepClone()`, `TemplateRegistry` (inregistrare + `GetClone(key)`).

Clonarea este profunda: sectiunile si `FormattingSettings` sunt copii independente.

### Singleton

**Problema:** configuratia aplicatiei trebuie citita o singura data, accesibila global.

**Unde:** `AppConfiguration.Instance` cu `Lazy<AppConfiguration>` — incarca `appsettings.json` la primul acces.

**De ce Singleton aici:** exista o singura sursa de configuratie per proces; accesul este read-only dupa incarcare; overhead minim.

**Cand `AddSingleton` in IoC:** aplicatii mari cu container DI — inlocuieste Singleton manual, permite testare cu configuratie mock, respecta DIP.

## Flux aplicatie

1. `TemplateRegistry` — clone sablon (`report` / `invoice`)
2. `DocumentDataBuilder` — metadate document
3. `DocumentAssembler` + factory raport/factura — componente
4. `DocumentExporter` — export HTML sau text in `output/`

## Teste

`dotnet test` — Factory Method, Abstract Factory, Builder, Prototype (6 teste).
