using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Yohuke.LiveMarker.Models;
using Yohuke.LiveMarker.Views;
using SettingsWindow = Yohuke.LiveMarker.Views.Windows.SettingsWindow;

namespace Yohuke.LiveMarker.ViewModels;

public partial class SettingsWindowViewModel : ViewModelBase<SettingsWindow>
{
    [ObservableProperty] private bool enableAutoSave;
    [ObservableProperty] private bool showDateTimeColumn;
    [ObservableProperty] private string selectedLanguage;

    public List<LanguageOption> Languages { get; } =
    [
        new("en", "English"),
        new("zh", "中文"),
        new("ja", "日本語")
    ];

    public SettingsWindowViewModel() {}

    public SettingsWindowViewModel(SettingsWindow window) : base(window)
    {
        var settings = AppRuntime.Settings;
        EnableAutoSave = settings.EnableAutoSave;
        ShowDateTimeColumn = settings.ShowDateTimeColumn;
        SelectedLanguage = settings.Language ?? "en";
    }

    [RelayCommand]
    private async Task Save()
    {
        var settings = AppRuntime.Settings;
        settings.EnableAutoSave = EnableAutoSave;
        settings.ShowDateTimeColumn = ShowDateTimeColumn;
        settings.Language = SelectedLanguage;
        await settings.Save();
        AppRuntime.I18N.SetLang(SelectedLanguage);
        View.Close(true);
    }

    [RelayCommand]
    private void Cancel()
    {
        View.Close(false);
    }
}