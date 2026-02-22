using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;

namespace Yohuke.LiveMarker.Utilities;

public static class StoragePickerUtilities
{
    public static async Task<string?> PickSaveFileAsync(
        Window window,
        string title,
        string defaultExtension,
        IReadOnlyList<FilePickerFileType> fileTypes)
    {
        var file = await window.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = title,
            DefaultExtension = defaultExtension,
            FileTypeChoices = fileTypes
        });

        return file?.TryGetLocalPath();
    }

    public static async Task<string?> PickOpenFileAsync(
        Window window,
        string title,
        IReadOnlyList<FilePickerFileType> fileTypes)
    {
        var files = await window.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = title,
            AllowMultiple = false,
            FileTypeFilter = fileTypes
        });

        return files is { Count: > 0 } ? files[0].TryGetLocalPath() : null;
    }

    // 常用文件类型定义
    public static class FileTypes
    {
        public static readonly FilePickerFileType Yaml =
            new("YAML") { Patterns = ["*.yaml", "*.yml"] };

        public static readonly FilePickerFileType PlainText =
            new("Text") { Patterns = ["*.txt", "*.tsv"] };

        public static readonly FilePickerFileType Excel =
            new("Excel") { Patterns = ["*.xlsx"] };

        public static readonly FilePickerFileType All =
            new("All files") { Patterns = ["*"] };
    }
}