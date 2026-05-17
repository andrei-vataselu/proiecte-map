# Task Manager — Laborator 3 (SOLID)

Aplicație .NET de gestiune a sarcinilor: SQLite, repository pattern, WinForms și principii SRP, OCP, LSP aplicate explicit.

## Structură

```
TaskManager.sln
├── src/
│   ├── TaskManager.Core/       — modele, interfețe, servicii, validatori, notificări
│   ├── TaskManager.Data/       — SqliteTaskRepository, InMemoryTaskRepository
│   └── TaskManager.UI/         — WinForms (MainForm, TaskEditForm)
└── tests/
    └── TaskManager.Tests/      — NUnit (doar InMemoryTaskRepository)
```

## Cerințe SOLID — justificare

### SRP (Cerința 1)

| Clasă                      | Actor / responsabilitate                                              |
| -------------------------- | --------------------------------------------------------------------- |
| `MainForm`, `TaskEditForm` | Prezentare: afișare, filtre, evenimente → apelează doar `TaskService` |
| `TaskService`              | Orchestrare: CRUD, filtrare, finalizare + notificare                  |
| `TaskValidator`            | Validare reguli (titlu, due date pentru deadline)                     |
| `SqliteTaskRepository`     | Persistență SQLite                                                    |
| `*Notifier`                | Livrare notificări la finalizare                                      |

O schimbare a UI-ului (ex. WPF) nu afectează `TaskService`. O schimbare a schemei DB nu afectează validatorul.

### OCP (Cerința 2)

`ITaskNotifier` + implementări (`EmailNotifier`, `ConsoleNotifier`, `FileLogNotifier`, `SlackNotifier`).  
`TaskService` primește `IReadOnlyDictionary<NotificationType, ITaskNotifier>` și selectează notifier-ul fără `switch` pe tip.

`SlackNotifier` demonstrează extensibilitatea: adăugat și înregistrat în `Program.cs` fără modificarea claselor existente.

### LSP + DbC (Cerința 3)

`TaskItem.Complete()` — Template Method: verifică precondiția (nu e deja Done), apelează `CompleteCore()`, verifică postcondiția (`Status == Done`) și invarianta (nu Done + Overdue).

- `RecurringTask`: Done + recalculează `DueDate`
- `DeadlineTask`: Done, comportament predictibil

### Repository (Cerința 4)

`ITaskRepository` — `SqliteTaskRepository` (mapare manuală din `SqliteDataReader`) și `InMemoryTaskRepository` (teste). Ambele sunt interschimbabile pentru `TaskService`.

## Rulare

**Cerințe:** .NET 6 SDK, Windows (WinForms).

```bash
# Restaurare și teste
dotnet restore
dotnet test

# Aplicație desktop
dotnet run --project src/TaskManager.UI/TaskManager.UI.csproj
```

Baza `tasks.db` se creează automat lângă executabil. Log notificări FileLog: `tasks.log`.

### Pachete NuGet

- `Microsoft.Data.Sqlite` (TaskManager.Data)
- `NUnit`, `NUnit3TestAdapter`, `Microsoft.NET.Test.Sdk` (TaskManager.Tests)

## UI

- `DataGridView` — listă sarcini
- ComboBox — filtrare status / prioritate
- Butoane: Adaugă, Editează, Șterge, Finalizează (apelează `CompleteTask` + notificare OCP)

## Laborator 4

Proiectul rămâne pregătit pentru: segregarea interfeței `ITaskRepository` (ISP) și înlocuirea compoziției manuale din `Program.cs` cu un container DI.
