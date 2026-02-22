// Yohuke.LiveMarker/ViewModels/MainWindowViewModel.Commands.cs
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using Yohuke.LiveMarker.Actions;
using Yohuke.LiveMarker.Models;
using Yohuke.LiveMarker.Utilities;
using Yohuke.LiveMarker.Views;

namespace Yohuke.LiveMarker.ViewModels;

public partial class MainWindowViewModel : ViewModelBase<MainWindow>
{
    [RelayCommand]
    private async Task AddMarker()
    {
        if (string.IsNullOrWhiteSpace(CurrentInputMessage))
        {
            return;
        }

        manuallyChangingInputTime = false;
        var d = new MarkerData
        {
            Message = CurrentInputMessage,
            RealDateTime = InputTime,
            MarkerColor = CurrentSelectedColor,
            LiveTime = InputTime - Data.StartTime
        };

        d.PropertyChanged += OnDataPropertyChanged;

        var action = new AddMarkerAction(Data.Marker, d);
        ActionManager.ExecuteAction(action);

        CurrentInputMessage = string.Empty;
        await AutoSave();
    }

    [RelayCommand]
    private async Task Create()
    {
        var path = await StoragePickerUtilities.PickSaveFileAsync(
            View, "Create", "yaml",
            [StoragePickerUtilities.FileTypes.Yaml, StoragePickerUtilities.FileTypes.All]);

        if (!string.IsNullOrEmpty(path))
        {
            await CreateInternal(path);
        }
    }
    
    [RelayCommand]
    private void ResetInputTime()
    {
        manuallyChangingInputTime = true;
        InputTime = DateTime.Now;
    }
    
    [RelayCommand]
    private async Task ResetStartTime()
    {
        var box = MessageBoxManager.GetMessageBoxStandard("Reset Start Time", "Are you sure you want to reset the start time? This will recalculate the live time for all markers.", ButtonEnum.YesNo);

        if (await box.ShowAsPopupAsync(View) == ButtonResult.Yes)
        {
            Data.StartTime = DateTime.Now;
            Data.CalculateLiveTime();
        }
    }
    
    [RelayCommand]
    private async Task Save()
    {
        var path = await StoragePickerUtilities.PickSaveFileAsync(
            View, "Save markers", "yaml",
            [StoragePickerUtilities.FileTypes.Yaml, StoragePickerUtilities.FileTypes.All]);

        if (!string.IsNullOrWhiteSpace(path))
        {
            await SaveInternal(path);
        }
    }
    
    [RelayCommand]
    private async Task Load()
    {
        var path = await StoragePickerUtilities.PickOpenFileAsync(
            View, "Load markers",
            [StoragePickerUtilities.FileTypes.Yaml, StoragePickerUtilities.FileTypes.All]);

        if (!string.IsNullOrWhiteSpace(path))
        {
            await LoadInternal(path);
        }
    }
    
    [RelayCommand]
    private async Task QuickSave()
    {
        if (string.IsNullOrWhiteSpace(CurrentFileLocation))
        {
            await Save();
        }
        else
        {
            await SaveInternal(CurrentFileLocation);
        }
    }
    
    [RelayCommand]
    private async Task ExportText()
    {
        var path = await StoragePickerUtilities.PickSaveFileAsync(
            View, "Export as Text", "txt",
            [StoragePickerUtilities.FileTypes.PlainText, StoragePickerUtilities.FileTypes.All]);

        if (!string.IsNullOrWhiteSpace(path))
        {
            await ExportInternal(true, path);
        }
    }
    
    [RelayCommand]
    private async Task ExportExcel()
    {
        var path = await StoragePickerUtilities.PickSaveFileAsync(
            View, "Export as Excel", "xlsx",
            [StoragePickerUtilities.FileTypes.Excel, StoragePickerUtilities.FileTypes.All]);

        if (!string.IsNullOrWhiteSpace(path))
        {
            await ExportInternal(false, path);
        }
    }
    
    [RelayCommand]
    private async Task DeleteMarker(MarkerData marker)
    {
        if (marker != null)
        {
            marker.PropertyChanged -= OnDataPropertyChanged;

            var action = new DeleteMarkerAction(Data.Marker, marker);
            ActionManager.ExecuteAction(action);

            await AutoSave();
        }
    }
    
    [RelayCommand]
    private async Task Undo()
    {
        if (!ActionManager.CanUndo) return;

        isActionInprogress = true;
        try
        {
            ActionManager.Undo();
            Data.CalculateLiveTime();
            await AutoSave();
        }
        finally
        {
            isActionInprogress = false;
        }
    }
    
    [RelayCommand]
    private async Task Redo()
    {
        if (!ActionManager.CanRedo) return;

        isActionInprogress = true;
        try
        {
            ActionManager.Redo();
            Data.CalculateLiveTime();
            await AutoSave();
        }
        finally
        {
            isActionInprogress = false;
        }
    }
    
    [RelayCommand]
    private async Task ShowAbout()
    {
        var box = MessageBoxManager.GetMessageBoxStandard("About",
            $"By 夜更けのシンフォニー(yosymph.com)\nVersion: {GetType().Assembly.GetName().Version}\nOpen source under GPLv3 License.");
        await box.ShowAsPopupAsync(View);
    }
    
    [RelayCommand]
    private void SelectColor(string parameter)
    {
        if (int.TryParse(parameter, out var index) && index >= 1 && index <= 7)
        {
            var colors = Enum.GetValues<MarkerColor>();
            if (index <= colors.Length)
            {
                CurrentSelectedColor = colors[index - 1];
            }
        }
    }

    [RelayCommand]
    private void Exit()
    {
        View.Close();
    }
}