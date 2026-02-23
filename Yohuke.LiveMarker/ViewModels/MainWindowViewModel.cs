using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Avalonia.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using MsBox.Avalonia;
using Yohuke.LiveMarker.Actions;
using Yohuke.LiveMarker.Exporters;
using Yohuke.LiveMarker.Models;
using Yohuke.LiveMarker.Utilities;
using Yohuke.LiveMarker.Views;

namespace Yohuke.LiveMarker.ViewModels;

public partial class MainWindowViewModel : ViewModelBase<MainWindow>
{
    [ObservableProperty] private LiveMarkerData data;
    [ObservableProperty] private MarkerColor currentSelectedColor = MarkerColorUtilities.DefaultColor.Color;
    [ObservableProperty] private string currentInputMessage;
    [ObservableProperty] private DateTime inputRealTime = DateTime.Now;
    [ObservableProperty] private TimeSpan inputLiveTime = new (0, 0, 0);
    [ObservableProperty] private bool isLoading = false;
    [ObservableProperty] private string currentFileLocation;
    [ObservableProperty] private bool useInputRealTime = true;

    partial void OnUseInputRealTimeChanged(bool value)
    {
        OnPropertyChanged(nameof(UseInputLiveTime));
    }

    public bool UseInputLiveTime
    {
        get => !UseInputRealTime;
        set => UseInputRealTime = !value;
    }

    [ObservableProperty] private bool showDateTimeColumn = AppRuntime.Settings?.ShowDateTimeColumn ?? true;

    public ActionManager ActionManager { get; } = new();

    private MarkerData editingSnapshot;
    private MarkerData editingMarker;
    private bool isActionInprogress;
    private bool isEditing;
    private bool lockingInputTime;

    private async Task ShowErrorAsync(string message)
    {
        var box = MessageBoxManager.GetMessageBoxStandard("Error", message);
        await box.ShowWindowDialogAsync(View);
    }

    private bool isApplyingParsedTime;

    partial void OnCurrentInputMessageChanged(string oldValue, string newValue)
    {
        // Auto-detect time pattern at the beginning of input and apply as LiveTime
        if (!isApplyingParsedTime &&
            !string.IsNullOrWhiteSpace(newValue) &&
            TimeUtilities.TryParseFlexibleTime(newValue, out var parsedTime, out var remaining))
        {
            isApplyingParsedTime = true;
            UseInputRealTime = false;
            InputLiveTime = parsedTime;
            CurrentInputMessage = remaining;
            isApplyingParsedTime = false;
            return;
        }

        if (lockingInputTime || isApplyingParsedTime)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(oldValue) && !string.IsNullOrWhiteSpace(newValue))
        {
            LockInputTime();
        }
    }

    private async Task AutoSave()
    {
        if (!string.IsNullOrWhiteSpace(CurrentFileLocation) && AppRuntime.Settings.EnableAutoSave)
        {
            await SaveInternal(CurrentFileLocation);
        }
    }

    private async void OnDataPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (isActionInprogress || isEditing) return;

        if (!string.IsNullOrWhiteSpace(CurrentFileLocation))
        {
            await SaveInternal(CurrentFileLocation);
        }
    }
    
    public void BeginEditMarker(MarkerData marker)
    {
        editingMarker = marker;
        editingSnapshot = marker?.CreateSnapshot();
        isEditing = true;}
    
    public async void CommitEditMarker()
    {
        isEditing = false;

        if (editingMarker != null && editingSnapshot != null)
        {
            var newSnapshot = editingMarker.CreateSnapshot();

            if (!editingSnapshot.IsEqual(newSnapshot))
            {
                var action = new EditMarkerAction(editingMarker, editingSnapshot, newSnapshot);
                ActionManager.ExecuteAction(action, executeNow: false);
            }
        }

        editingMarker = null;
        editingSnapshot = null;

        await AutoSave();
    }

    public void EndEditMarker()
    {
        isEditing = false;
        editingMarker = null;
        editingSnapshot = null;
    }

    private async Task SaveInternal(string path)
    {
        IsLoading = true;

        try
        {
            await Data.Save(path);
            CurrentFileLocation = path;
        }
        catch (Exception ex)
        {
            await ShowErrorAsync($"Failed to save: {ex.Message}");
        }

        IsLoading = false;
    }

    private async Task LoadInternal(string path)
    {
        IsLoading = true;
        try
        {
            var d = await LiveMarkerData.Load(path);
            CurrentFileLocation = path;

            if (d == null)
            {
                await ShowErrorAsync("File failed to load.");
                return;
            }

            Data = d;
            ActionManager.Clear();

            foreach (var marker in Data.Marker)
            {
                marker.PropertyChanged += OnDataPropertyChanged;
            }
            
            SetGridSortOrder();
        }
        catch (Exception ex)
        {
            await ShowErrorAsync($"Failed to load: {ex.Message}");
        }
        
        IsLoading = false;
    }

    private async Task CreateInternal(string path)
    {
        Data = new LiveMarkerData
        {
            StartTime = DateTime.Now
        };
        CurrentInputMessage = string.Empty;
        CurrentFileLocation = string.Empty;
        ActionManager.Clear();

        await SaveInternal(path);
    }

    public void SetGridSortOrder()
    {
        var view = new DataGridCollectionView(Data.Marker);
        view.SortDescriptions.Add(DataGridSortDescription.FromPath(nameof(MarkerData.RealDateTime)));

        View.MarkerDataGrid.ItemsSource = view;
    }
    
    private async Task ExportInternal(bool isText, string path)
    {
        IsLoading = true;

        try
        {
            IMarkerExporter exporter = isText ? new PlainTextExporter() : new ExcelExporter();
            await exporter.Export(Data, path);
        }
        catch (Exception ex)
        {
            await ShowErrorAsync($"Failed to export: {ex.Message}");
        }

        IsLoading = false;
    }

    public MainWindowViewModel(MainWindow window) : base(window)
    {
        Data = new()
        {
            StartTime = DateTime.Now.TruncateMilliseconds()
        };
    }

    public MainWindowViewModel() { }
}
