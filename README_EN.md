# Live Marker

A cross-platform desktop application for real-time event marking during live streams, meetings, or any timed sessions. Built with [Avalonia UI](https://avaloniaui.net/) and .NET 9.

**Author:** 夜更けのシンフォニー ([yosymph.com](https://yosymph.com))  
**License:** GPLv3

**[中文文档](README.md)**

---

![screenshot.png](screenshot.png)

## Features

- ⏱ **Real-time & Live-time marking** — Record markers with either the actual clock time or elapsed time from a configurable start point.
- 🎨 **Color-coded markers** — 7 built-in colors (Red, Orange, Yellow, Green, Blue, Magenta, Grey) with keyboard shortcuts for quick selection.
- ⌨️ **Smart time input** — Type a time (e.g. `1:23:45`, `1.23.45`, `1：23：45`, `1-23-45`) at the beginning of your message and it will be automatically parsed as Live Time.
- 💾 **Auto-save** — Automatically saves your work when a file is open (configurable in Settings).
- ↩️ **Undo / Redo** — Full undo/redo support for add, delete, and edit actions.
- 📤 **Export** — Export markers to Plain Text (`.txt`) or Excel (`.xlsx`) with color highlighting.
- 🖥 **Cross-platform** — Runs on Windows (x64/ARM64), Linux (x64), and macOS (ARM64).

---

## User Guide

### Getting Started

1. Download the release for your platform from the Releases page.
2. Run `LiveMarker` (or `LiveMarker.exe` on Windows).
3. The start time is automatically set to the current time when the application launches.

### Adding Markers

1. Type your message in the input box at the bottom.
2. Press <kbd>Enter</kbd> to add the marker.
3. The timestamp is automatically captured when you start typing.

### Time Modes

- **Real Time mode** (default) — The marker records the actual clock time when you started typing.
- **Live Time mode** — The marker records the elapsed time since the start time.
- Click the ⏱ timer button (or press <kbd>Ctrl+T</kbd> / <kbd>⌘T</kbd>) to toggle between modes.

### Smart Time Input

You can type a time at the beginning of your message to manually specify the Live Time:

| Format | Example |
|---|---|
| `hh:mm:ss` | `1:23:45 some event` |
| `hh.mm.ss` | `1.23.45 some event` |
| `hh：mm：ss` | `1：23：45 some event` (full-width colon) |
| `hh-mm-ss` | `1-23-45 some event` |

The time will be automatically extracted and applied; the remaining text stays as the message.

### Keyboard Shortcuts

| Action | Windows / Linux | macOS |
|---|---|---|
| Add Marker | <kbd>Enter</kbd> | <kbd>Enter</kbd> |
| Lock Input Time | <kbd>Escape</kbd> | <kbd>Escape</kbd> |
| Unlock Input Time | <kbd>Shift+Escape</kbd> | <kbd>Shift+Escape</kbd> |
| Switch Time Mode | <kbd>Ctrl+T</kbd> | <kbd>⌘T</kbd> |
| Create | <kbd>Ctrl+N</kbd> | <kbd>⌘N</kbd> |
| Open | <kbd>Ctrl+O</kbd> | <kbd>⌘O</kbd> |
| Save | <kbd>Ctrl+S</kbd> | <kbd>⌘S</kbd> |
| Save As | <kbd>Shift+Ctrl+S</kbd> | <kbd>⇧⌘S</kbd> |
| Undo | <kbd>Ctrl+Z</kbd> | <kbd>⌘Z</kbd> |
| Redo | <kbd>Ctrl+Y</kbd> | <kbd>⌘Y</kbd> |
| Settings | <kbd>Shift+Ctrl+P</kbd> | <kbd>⇧⌘P</kbd> |
| Export to Text | <kbd>Shift+Ctrl+T</kbd> | <kbd>⇧⌘T</kbd> |
| Export to Excel | <kbd>Shift+Ctrl+E</kbd> | <kbd>⇧⌘E</kbd> |
| Select Color 1–7 | <kbd>Ctrl+1</kbd> ~ <kbd>Ctrl+7</kbd> | <kbd>⌘1</kbd> ~ <kbd>⌘7</kbd> |
| Delete Marker | <kbd>Delete</kbd> (in table) | <kbd>Delete</kbd> (in table) |

### Settings

Access via **File → Settings** or <kbd>Shift+Ctrl+P</kbd> / <kbd>⇧⌘P</kbd>:

- **Enable Auto Save** — Automatically save when changes are made (default: on).
- **Show Date Time Column** — Show/hide the absolute date-time column in the marker table (default: on).

### File Format

Markers are saved in YAML (`.yaml`) format for easy readability and version control.

---

## Developer Guide

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- Any IDE that supports .NET development (Rider, Visual Studio, VS Code)

### Tech Stack

| Component | Technology |
|---|---|
| UI Framework | [Avalonia UI](https://avaloniaui.net/) 11.3 |
| Architecture | MVVM with [CommunityToolkit.Mvvm](https://github.com/CommunityToolkit/dotnet) |
| Data Serialization | [YamlDotNet](https://github.com/aaubry/YamlDotNet) |
| Excel Export | [NPOI](https://github.com/nissl-lab/npoi) |
| Icons | [Icons.Avalonia](https://github.com/Projektanker/Icons.Avalonia) |
| Dialogs | [MessageBox.Avalonia](https://github.com/AvaloniaCommunity/MessageBox.Avalonia) |

### Project Structure

```
Yohuke.LiveMarker/
├── Actions/              # Undo/redo action system
│   ├── ActionManager.cs
│   ├── IUndoableAction.cs
│   ├── AddMarkerAction.cs
│   ├── DeleteMarkerAction.cs
│   └── EditMarkerAction.cs
├── Assets/               # Application icons
├── Converters/           # Avalonia value converters
│   ├── LiveTimeConverter.cs
│   ├── LiveTimeMultiConverter.cs
│   ├── MarkerColorToBrushConverter.cs
│   └── MarkerColorToDefinitionConverter.cs
├── Exporters/            # Export functionality
│   ├── IMarkerExporter.cs
│   ├── PlainTextExporter.cs
│   └── ExcelExporter.cs
├── Models/               # Data models
│   ├── LiveMarkerData.cs
│   ├── MarkerData.cs
│   ├── MarkColor.cs
│   └── MarkerColorDefinition.cs
├── Settings/             # App settings & local storage
│   ├── AppSettings.cs
│   ├── LocalCache.cs
│   └── LocalPreference.cs
├── Utilities/            # Helper classes
│   ├── MarkerColorUtilities.cs
│   ├── PathUtilities.cs
│   ├── StoragePickerUtilities.cs
│   └── TimeUtilities.cs
├── ViewModels/           # MVVM ViewModels
│   ├── ViewModelBase.cs
│   ├── MainWindowViewModel.cs
│   ├── MainWindowViewModel.Commands.cs
│   └── SettingsWindowViewModel.cs
├── Views/                # Avalonia UI views
│   ├── MainWindow.axaml / .axaml.cs
│   ├── SettingsWindow.axaml / .axaml.cs
│   └── ColorChoiceCombo.axaml / .axaml.cs
├── App.axaml / .axaml.cs
├── AppRuntime.cs         # Global runtime state
└── Program.cs            # Entry point
```

### Build & Run

```bash
# Clone the repository
git clone https://github.com/raphaelstuart/Yohuke.LiveMarker.git
cd yohuke.livemarker

# Restore and run
dotnet run --project Yohuke.LiveMarker
```

### Publish

A PowerShell script is provided to build self-contained single-file executables for all supported platforms:

```powershell
# Requires PowerShell
./publish.ps1
```

This publishes to `./publish/<platform>/` for:
- `win-x64`
- `win-arm64`
- `linux-x64`
- `osx-arm64`

Each build uses `--self-contained true -p:PublishSingleFile=true`, so no .NET runtime installation is required on the target machine.

### Architecture Notes

- **MVVM pattern** — Views bind to ViewModels via compiled bindings (`x:DataType`). `ViewModelBase<T>` provides a typed reference to the owning Window.
- **Undo/Redo** — Implemented via the Command pattern. `ActionManager` maintains undo/redo stacks of `IUndoableAction` objects.
- **Partial classes** — `MainWindowViewModel` is split into `MainWindowViewModel.cs` (state & logic) and `MainWindowViewModel.Commands.cs` (command definitions) for readability.
- **Smart time parsing** — `TimeUtilities.TryParseFlexibleTime()` uses regex to detect and extract time patterns from user input, supporting multiple separator styles.
- **Settings** — Persisted as YAML via `AppSettings`, loaded at startup through `AppRuntime.Init()`.
