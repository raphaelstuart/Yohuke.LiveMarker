# Live Marker

跨平台桌面应用，用于直播、会议或任何计时场景中的实时事件标记。基于 [Avalonia UI](https://avaloniaui.net/) 和 .NET 9 构建。

**作者：** 夜更けのシンフォニー ([yosymph.com](https://yosymph.com))  
**许可证：** GPLv3

**[English](README.md)**

---

## 功能特性

- ⏱ **实际时间与直播时间标记** —— 使用实际时钟时间或从可配置起始点算起的经过时间记录标记。
- 🎨 **颜色标记** —— 7 种内置颜色（红、橙、黄、绿、蓝、紫、灰），支持快捷键快速选择。
- ⌨️ **智能时间输入** —— 在消息开头输入时间（如 `1:23:45`、`1.23.45`、`1：23：45`、`1-23-45`），将自动识别为直播时间。
- 💾 **自动保存** —— 打开文件后自动保存（可在设置中配置）。
- ↩️ **撤销 / 重做** —— 完整支持添加、删除、编辑操作的撤销与重做。
- 📤 **导出** —— 导出标记为纯文本（`.txt`）或带颜色高亮的 Excel（`.xlsx`）。
- 🖥 **跨平台** —— 支持 Windows（x64/ARM64）、Linux（x64）和 macOS（ARM64）。

---

## 使用说明

### 快速开始

1. 从 Releases 页面下载对应平台的版本。
2. 运行 `LiveMarker`（Windows 上为 `LiveMarker.exe`）。
3. 应用启动时会自动将当前时间设为起始时间。

### 添加标记

1. 在底部输入框中输入消息内容。
2. 按 <kbd>Enter</kbd> 添加标记。
3. 开始输入时会自动捕获当前时间戳。

### 时间模式

- **实际时间模式**（默认）—— 标记记录开始输入时的实际时钟时间。
- **直播时间模式** —— 标记记录从起始时间算起的经过时间。
- 点击底部 ⏱ 按钮（或按 <kbd>Ctrl+T</kbd> / <kbd>⌘T</kbd>）切换模式。

### 智能时间输入

在消息开头输入时间，将自动识别为直播时间（Live Time）：

| 格式 | 示例 |
|---|---|
| `hh:mm:ss` | `1:23:45 某个事件` |
| `hh.mm.ss` | `1.23.45 某个事件` |
| `hh：mm：ss` | `1：23：45 某个事件`（全角冒号） |
| `hh-mm-ss` | `1-23-45 某个事件` |

时间会被自动提取并应用，剩余文本保留为消息内容。

### 快捷键

| 操作 | Windows / Linux | macOS |
|---|---|---|
| 添加标记 | <kbd>Enter</kbd> | <kbd>Enter</kbd> |
| 锁定输入时间 | <kbd>Escape</kbd> | <kbd>Escape</kbd> |
| 解锁输入时间 | <kbd>Shift+Escape</kbd> | <kbd>Shift+Escape</kbd> |
| 切换时间模式 | <kbd>Ctrl+T</kbd> | <kbd>⌘T</kbd> |
| 新建 | <kbd>Ctrl+N</kbd> | <kbd>⌘N</kbd> |
| 打开 | <kbd>Ctrl+O</kbd> | <kbd>⌘O</kbd> |
| 保存 | <kbd>Ctrl+S</kbd> | <kbd>⌘S</kbd> |
| 另存为 | <kbd>Shift+Ctrl+S</kbd> | <kbd>⇧⌘S</kbd> |
| 撤销 | <kbd>Ctrl+Z</kbd> | <kbd>⌘Z</kbd> |
| 重做 | <kbd>Ctrl+Y</kbd> | <kbd>⌘Y</kbd> |
| 设置 | <kbd>Shift+Ctrl+P</kbd> | <kbd>⇧⌘P</kbd> |
| 导出为文本 | <kbd>Shift+Ctrl+T</kbd> | <kbd>⇧⌘T</kbd> |
| 导出为 Excel | <kbd>Shift+Ctrl+E</kbd> | <kbd>⇧⌘E</kbd> |
| 选择颜色 1–7 | <kbd>Ctrl+1</kbd> ~ <kbd>Ctrl+7</kbd> | <kbd>⌘1</kbd> ~ <kbd>⌘7</kbd> |
| 删除标记 | <kbd>Delete</kbd>（在表格中） | <kbd>Delete</kbd>（在表格中） |

### 设置

通过 **File → Settings** 或 <kbd>Shift+Ctrl+P</kbd> / <kbd>⇧⌘P</kbd> 访问：

- **启用自动保存** —— 修改时自动保存（默认开启）。
- **显示日期时间列** —— 在标记表格中显示/隐藏绝对日期时间列（默认开启）。

### 文件格式

标记以 YAML（`.yaml`）格式保存，便于阅读和版本管理。

---

## 开发者说明

### 前置要求

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- 任何支持 .NET 开发的 IDE（Rider、Visual Studio、VS Code）

### 技术栈

| 组件 | 技术 |
|---|---|
| UI 框架 | [Avalonia UI](https://avaloniaui.net/) 11.3 |
| 架构模式 | MVVM，使用 [CommunityToolkit.Mvvm](https://github.com/CommunityToolkit/dotnet) |
| 数据序列化 | [YamlDotNet](https://github.com/aaubry/YamlDotNet) |
| Excel 导出 | [NPOI](https://github.com/nissl-lab/npoi) |
| 图标 | [FluentIcons.Avalonia](https://github.com/nicokimmel/FluentIcons.Avalonia) |
| 对话框 | [MessageBox.Avalonia](https://github.com/AvaloniaCommunity/MessageBox.Avalonia) |

### 项目结构

```
Yohuke.LiveMarker/
├── Actions/              # 撤销/重做操作系统
│   ├── ActionManager.cs
│   ├── IUndoableAction.cs
│   ├── AddMarkerAction.cs
│   ├── DeleteMarkerAction.cs
│   └── EditMarkerAction.cs
├── Assets/               # 应用图标
├── Converters/           # Avalonia 值转换器
│   ├── LiveTimeConverter.cs
│   ├── LiveTimeMultiConverter.cs
│   ├── MarkerColorToBrushConverter.cs
│   └── MarkerColorToDefinitionConverter.cs
├── Exporters/            # 导出功能
│   ├── IMarkerExporter.cs
│   ├── PlainTextExporter.cs
│   └── ExcelExporter.cs
├── Models/               # 数据模型
│   ├── LiveMarkerData.cs
│   ├── MarkerData.cs
│   ├── MarkColor.cs
│   └── MarkerColorDefinition.cs
├── Settings/             # 应用设置与本地存储
│   ├── AppSettings.cs
│   ├── LocalCache.cs
│   └── LocalPreference.cs
├── Utilities/            # 工具类
│   ├── MarkerColorUtilities.cs
│   ├── PathUtilities.cs
│   ├── StoragePickerUtilities.cs
│   └── TimeUtilities.cs
├── ViewModels/           # MVVM ViewModel 层
│   ├── ViewModelBase.cs
│   ├── MainWindowViewModel.cs
│   ├── MainWindowViewModel.Commands.cs
│   └── SettingsWindowViewModel.cs
├── Views/                # Avalonia UI 视图层
│   ├── MainWindow.axaml / .axaml.cs
│   ├── SettingsWindow.axaml / .axaml.cs
│   └── ColorChoiceCombo.axaml / .axaml.cs
├── App.axaml / .axaml.cs
├── AppRuntime.cs         # 全局运行时状态
└── Program.cs            # 程序入口
```

### 构建与运行

```bash
# 克隆仓库
git clone https://github.com/raphaelstuart/Yohuke.LiveMarker.git
cd yohuke.livemarker

# 还原依赖并运行
dotnet run --project Yohuke.LiveMarker
```

### 发布

提供了 PowerShell 脚本，可一键构建所有平台的独立单文件可执行程序：

```powershell
# 需要 PowerShell
./publish.ps1
```

将发布到 `./publish/<平台>/`，支持：
- `win-x64`
- `win-arm64`
- `linux-x64`
- `osx-arm64`

使用 `--self-contained true -p:PublishSingleFile=true` 构建，目标机器无需安装 .NET 运行时。

### 架构说明

- **MVVM 模式** —— 视图通过编译绑定（`x:DataType`）绑定到 ViewModel。`ViewModelBase<T>` 提供对宿主窗口的类型化引用。
- **撤销/重做** —— 基于命令模式实现。`ActionManager` 维护 `IUndoableAction` 对象的撤销/重做栈。
- **分部类** —— `MainWindowViewModel` 拆分为 `.cs`（状态与逻辑）和 `.Commands.cs`（命令定义），提升可读性。
- **智能时间解析** —— `TimeUtilities.TryParseFlexibleTime()` 使用正则表达式检测并提取用户输入中的时间格式，支持多种分隔符（`:` `.` `：` `-`）。
- **设置持久化** —— 通过 `AppSettings` 以 YAML 格式存储，启动时由 `AppRuntime.Init()` 加载。

