using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using Yohuke.LiveMarker.Actions;
using Yohuke.LiveMarker.Models;
using Yohuke.LiveMarker.Utilities;
using Yohuke.LiveMarker.Views;
using MainWindow = Yohuke.LiveMarker.Views.Windows.MainWindow;
using OffsetTimeWindow = Yohuke.LiveMarker.Views.Windows.OffsetTimeWindow;
using SettingsWindow = Yohuke.LiveMarker.Views.Windows.SettingsWindow;

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

        lockingInputTime = false;
        var d = new MarkerData
        {
            Message = CurrentInputMessage,
            MarkerColor = CurrentSelectedColor,
        };

        if (UseInputRealTime)
        {
            d.RealDateTime = InputRealTime;
            d.LiveTime = InputRealTime - Data.StartTime;
        }
        else
        {
            d.LiveTime = InputLiveTime;
            d.RealDateTime = Data.StartTime + InputLiveTime;
        }

        d.PropertyChanged += OnDataPropertyChanged;

        var action = new AddMarkerAction(Data.Marker, d);
        ActionManager.ExecuteAction(action);

        CurrentInputMessage = string.Empty;
        await AutoSave();
    }
    
    [RelayCommand]
    private void SwitchMode()
    {
        UseInputRealTime = !UseInputRealTime;
    }

    [RelayCommand]
    private async Task Create()
    {
        var path = await StoragePickerUtilities.PickSaveFileAsync(
            View, AppRuntime.I18N.GetText("Picker_Create"), "yaml",
            [StoragePickerUtilities.FileTypes.Yaml, StoragePickerUtilities.FileTypes.All]);

        if (!string.IsNullOrEmpty(path))
        {
            await CreateInternal(path);
        }
    }
    
    [RelayCommand]
    private void LockInputTime()
    {
        lockingInputTime = true;
        InputRealTime = DateTime.Now;
        InputLiveTime = DateTime.Now - Data.StartTime;
    }
    
    [RelayCommand]
    private void UnlockInputTime()
    {
        lockingInputTime = false;
        InputRealTime = DateTime.Now;
        InputLiveTime = DateTime.Now - Data.StartTime;
    }
    
    [RelayCommand]
    private async Task ResetStartTime()
    {
        var box = MessageBoxManager.GetMessageBoxStandard(AppRuntime.I18N.GetText("Dialog_ResetStartTime_Title"), AppRuntime.I18N.GetText("Dialog_ResetStartTime_Message"), ButtonEnum.YesNo);

        if (await box.ShowWindowDialogAsync(View) == ButtonResult.Yes)
        {
            Data.StartTime = DateTime.Now;
            Data.CalculateLiveTime();
        }
    }
    
    [RelayCommand]
    private async Task Save()
    {
        if (string.IsNullOrWhiteSpace(CurrentFileLocation))
        {
            await SaveAs();
        }
        else
        {
            await SaveInternal(CurrentFileLocation);
        }
    }
    
    [RelayCommand]
    private async Task SaveAs()
    {
        var path = await StoragePickerUtilities.PickSaveFileAsync(
            View, AppRuntime.I18N.GetText("Picker_SaveAs"), "yaml",
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
            View, AppRuntime.I18N.GetText("Picker_Load"),
            [StoragePickerUtilities.FileTypes.Yaml, StoragePickerUtilities.FileTypes.All]);

        if (!string.IsNullOrWhiteSpace(path))
        {
            await LoadInternal(path);
        }
    }
    
    [RelayCommand]
    private async Task ExportText()
    {
        var path = await StoragePickerUtilities.PickSaveFileAsync(
            View, AppRuntime.I18N.GetText("Picker_ExportText"), "txt",
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
            View, AppRuntime.I18N.GetText("Picker_ExportExcel"), "xlsx",
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
            
            Data.Marker.Remove(marker);
            
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
        var box = MessageBoxManager.GetMessageBoxStandard(AppRuntime.I18N.GetText("Dialog_About_Title"),
            $"By 夜更けのシンフォニー(yosymph.com)\nVersion: {GetType().Assembly.GetName().Version}\nOpen source under GPLv3 License.");
        await box.ShowWindowDialogAsync(View);
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
    private async Task SetTimeOffset()
    {
        var popup = new OffsetTimeWindow();
        var result = await popup.ShowDialog<TimeSpan?>(View);
        if (result.HasValue && Data?.Marker != null && Data.Marker.Count > 0)
        {
            isActionInprogress = true;
            try
            {
                var action = new OffsetAllMarkersAction(Data.Marker, result.Value);
                ActionManager.ExecuteAction(action);
                await AutoSave();
            }
            finally
            {
                isActionInprogress = false;
            }
        }
    }

    [RelayCommand]
    private void Exit()
    {
        View.Close();
    }

    [RelayCommand]
    private async Task OpenSettings()
    {
        var popup = new SettingsWindow();
        await popup.ShowDialog(View);
        ShowDateTimeColumn = AppRuntime.Settings.ShowDateTimeColumn;
    }
}