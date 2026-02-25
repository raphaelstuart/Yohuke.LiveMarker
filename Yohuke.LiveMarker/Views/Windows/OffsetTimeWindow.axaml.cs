using Avalonia.Controls;
using Yohuke.LiveMarker.ViewModels;

namespace Yohuke.LiveMarker.Views.Windows;

public partial class OffsetTimeWindow : Window
{
    public OffsetTimeWindowViewModel ViewModel { get; }

    public OffsetTimeWindow()
    {
        ViewModel = new OffsetTimeWindowViewModel(this);
        InitializeComponent();
    }
}

