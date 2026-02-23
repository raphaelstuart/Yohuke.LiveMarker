using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Yohuke.LiveMarker.Settings;
using Yohuke.LiveMarker.Views;

namespace Yohuke.LiveMarker.ViewModels;

public partial class SettingsWindowViewModel : ViewModelBase<SettingsWindow>
{
    [ObservableProperty] private bool enableAutoSave;
    [ObservableProperty] private bool showDateTimeColumn;

    public SettingsWindowViewModel() {}

    public SettingsWindowViewModel(SettingsWindow window) : base(window)
    {
        var settings = AppRuntime.Settings;
        EnableAutoSave = settings.EnableAutoSave;
        ShowDateTimeColumn = settings.ShowDateTimeColumn;
    }

    [RelayCommand]
    private async Task Save()
    {
        var settings = AppRuntime.Settings;
        settings.EnableAutoSave = EnableAutoSave;
        settings.ShowDateTimeColumn = ShowDateTimeColumn;
        await settings.Save();
        View.Close(true);
    }

    [RelayCommand]
    private void Cancel()
    {
        View.Close(false);
    }
}


