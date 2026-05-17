# MusicPlayer — Laborator 8 (MAP)

Player desktop WPF cu **Observer**, **Strategy** și **Command**, redare audio reală prin **NAudio**.

## Cerințe îndeplinite

| Pattern | Implementare |
|---------|----------------|
| Observer | `AudioPlayer` (`INotifyPropertyChanged`, `TrackEnded`), `Playlist` (`ObservableCollection`), observatori: `MainWindowViewModel`, `PlaybackLogger`, `StatisticsTracker`, `AutoNextHandler` |
| Command | `IPlayerCommand`, comenzi play/pause/next/previous + modificări playlist, `CommandHistory` (undo/redo, max 50) |
| Strategy | `IPlaybackStrategy` — Secvențial, Aleator, Smart Shuffle, Repetă unul |

## Rulare

```bash
dotnet build MusicPlayer.sln
dotnet test MusicPlayer.sln
dotnet run --project src/MusicPlayer/MusicPlayer.csproj
```

În aplicație: **+ Adaugă fișiere…** → selectează MP3/WAV din `samples/` sau de pe disc.

## Structură

```
MusicPlayer.sln
├── src/MusicPlayer/     (WPF + MVVM)
└── tests/MusicPlayer.Tests/
```

## Diagramă clase (pattern-uri)

```mermaid
classDiagram
    class AudioPlayer {
        +INotifyPropertyChanged
        +TrackEnded
    }
    class Playlist {
        +Tracks
    }
    class IPlaybackStrategy {
        +GetNextTrack()
        +GetPreviousTrack()
    }
    class IPlayerCommand {
        +Execute()
        +Undo()
    }
    class CommandHistory {
        +Execute()
        +Undo()
        +Redo()
    }
    class PlaybackController
    class PlaybackLogger
    class StatisticsTracker
    class AutoNextHandler
    class MainWindowViewModel

    AudioPlayer <-- PlaybackController
    Playlist <-- PlaybackController
    IPlaybackStrategy <-- PlaybackController
    IPlayerCommand <-- CommandHistory
    AudioPlayer ..> MainWindowViewModel : observe
    AudioPlayer ..> PlaybackLogger : observe
    AudioPlayer ..> StatisticsTracker : observe
    AudioPlayer ..> AutoNextHandler : observe
```

## Secvență: Skip → Strategy → Player

```mermaid
sequenceDiagram
    participant U as Utilizator
    participant VM as MainWindowViewModel
    participant H as CommandHistory
    participant C as NextCommand
    participant PC as PlaybackController
    participant S as IPlaybackStrategy
    participant P as AudioPlayer

    U->>VM: Click Următorul
    VM->>H: Execute(NextCommand)
    H->>C: Execute()
    C->>PC: PlayNext()
    PC->>S: GetNextTrack(playlist, current)
    S-->>PC: Track următor
    PC->>P: Load(track) + Play()
    P-->>VM: PropertyChanged
```

## Capturi ecran

După rulare, fă capturi pentru:

1. Panoul **Acum se redă** cu o piesă încărcată
2. **Mod redare** cu altă strategie selectată
3. Panoul **Istoric** după câteva acțiuni undoable

Salvează imaginile în `docs/screenshots/` (opțional).

## Note

- Log redare: `playback_log.txt` lângă executabil
- `.NET 7` (SDK instalat); cod compatibil cu stil C# modern
- Fișierele audio nu sunt incluse în repo — vezi `samples/README.txt`
