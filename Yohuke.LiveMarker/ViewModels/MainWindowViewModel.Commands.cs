using System;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Yohuke.LiveMarker.Exporters;
using Yohuke.LiveMarker.Models;
using Yohuke.LiveMarker.Utilities;
using Yohuke.LiveMarker.Views;

namespace Yohuke.LiveMarker.ViewModels;

public partial class MainWindowViewModel : ViewModelBase<MainWindow>
{
    public IAsyncRelayCommand CreateCommand { get; private set; }
    public IAsyncRelayCommand AddMarkerCommand { get; private set; }
    public ICommand ResetInputTimeCommand { get; private set; }
    public ICommand ResetStartTimeCommand { get; private set; }
    public IAsyncRelayCommand SaveCommand { get; private set; }
    public IAsyncRelayCommand QuickSaveCommand { get; private set; }
    public IAsyncRelayCommand LoadCommand { get; private set; }
    public IAsyncRelayCommand ExportTextCommand { get; private set; }
    public IAsyncRelayCommand ExportExcelCommand { get; private set; }
    public ICommand DeleteMarkerCommand { get; private set; }

    private async Task AddMarker()
    {
        var d = new MarkerData
        {
            Message = CurrentInputMessage,
            RealDateTime = InputTime,
            MarkerColor = CurrentSelectedColor,
            LiveTime = InputTime - Data.StartTime
        };

        Data.Marker.Add(d);
        CurrentInputMessage = string.Empty;

        if (!string.IsNullOrWhiteSpace(CurrentFileLocation))
        {
            await SaveInternal(CurrentFileLocation);
        }
    }

    private async Task CreateNew()
    {
        var path = await StoragePickerUtilities.PickSaveFileAsync(
            View, "Create", "yaml",
            [StoragePickerUtilities.FileTypes.Yaml, StoragePickerUtilities.FileTypes.All]);

        if (!string.IsNullOrEmpty(path))
        {
            Data = new LiveMarkerData
            {
                StartTime = DateTime.Now
            };
            CurrentInputMessage = string.Empty;
            CurrentFileLocation = string.Empty;

            await SaveInternal(path);
        }
    }
    
    private void ResetInputTime()
    {
        InputTime = DateTime.Now;
    }

    private void ResetStartTime()
    {
        Data.StartTime = DateTime.Now;
        Data.CalculateLiveTime();
    }

    private async Task SaveAsync()
    {
        var path = await StoragePickerUtilities.PickSaveFileAsync(
            View, "Save markers", "yaml",
            [StoragePickerUtilities.FileTypes.Yaml, StoragePickerUtilities.FileTypes.All]);

        if (!string.IsNullOrWhiteSpace(path))
        {
            await SaveInternal(path);
        }
    }

    private async Task LoadAsync()
    {
        var path = await StoragePickerUtilities.PickOpenFileAsync(
            View, "Load markers",
            [StoragePickerUtilities.FileTypes.Yaml, StoragePickerUtilities.FileTypes.All]);

        if (!string.IsNullOrWhiteSpace(path))
        {
            await LoadInternal(path);
        }
    }
    
    private async Task QuickSaveAsync()
    {
        IsLoading = true;
        if (string.IsNullOrWhiteSpace(CurrentFileLocation))
        {
            await SaveAsync();
        }
        else
        {
            await SaveInternal(CurrentFileLocation);
        }
        IsLoading = false;
    }

    private async Task ExportTextAsync()
    {
        var path = await StoragePickerUtilities.PickSaveFileAsync(
            View, "Export as Text", "txt",
            [StoragePickerUtilities.FileTypes.PlainText, StoragePickerUtilities.FileTypes.All]);

        if (!string.IsNullOrWhiteSpace(path))
        {
            await ExportInternal(true, path);
        }
    }

    private async Task ExportExcelAsync()
    {
        var path = await StoragePickerUtilities.PickSaveFileAsync(
            View, "Export as Excel", "xlsx",
            [StoragePickerUtilities.FileTypes.Excel, StoragePickerUtilities.FileTypes.All]);

        if (!string.IsNullOrWhiteSpace(path))
        {
            await ExportInternal(false, path);
        }
    }

    private void DeleteMarker(MarkerData marker)
    {
        if (marker != null)
        {
            Data.Marker.Remove(marker);
        }
    }
    
    private void BindCommands()
    {
        CreateCommand = new AsyncRelayCommand(CreateNew);
        AddMarkerCommand = new AsyncRelayCommand(AddMarker);
        ResetInputTimeCommand = new RelayCommand(ResetInputTime);
        ResetStartTimeCommand = new RelayCommand(ResetStartTime);
        SaveCommand = new AsyncRelayCommand(SaveAsync);
        QuickSaveCommand = new AsyncRelayCommand(QuickSaveAsync);
        LoadCommand = new AsyncRelayCommand(LoadAsync);
        ExportTextCommand = new AsyncRelayCommand(ExportTextAsync);
        ExportExcelCommand = new AsyncRelayCommand(ExportExcelAsync);
        DeleteMarkerCommand = new RelayCommand<MarkerData>(DeleteMarker);
    }
}