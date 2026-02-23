using Avalonia.Controls;
using Yohuke.LiveMarker.ViewModels;

namespace Yohuke.LiveMarker.Views.Windows;

public partial class SettingsWindow : Window
{
    public SettingsWindowViewModel ViewModel { get; }

    public SettingsWindow()
    {
        ViewModel = new SettingsWindowViewModel(this);
        InitializeComponent();
    }
}