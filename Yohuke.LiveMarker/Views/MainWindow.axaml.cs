using Avalonia.Controls;
using Avalonia.Input;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using Yohuke.LiveMarker.Models;
using Yohuke.LiveMarker.ViewModels;

namespace Yohuke.LiveMarker.Views;

public partial class MainWindow : Window
{
    public MainWindowViewModel ViewModel { get; }
    public MainWindow()
    {
        ViewModel = new MainWindowViewModel(this);
        InitializeComponent();
    }
    
    private void MarkerDataGrid_OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Delete && sender is DataGrid grid)
        {
            if (grid.SelectedItem is MarkerData marker)
            {
                ViewModel.DeleteMarkerCommand.Execute(marker);
                e.Handled = true;
            }
        }
    }

    private bool isClosing;
    protected override async void OnClosing(WindowClosingEventArgs e)
    {
        if (isClosing)
        {
            return;
        }
        
        e.Cancel = true;

        var box = MessageBoxManager.GetMessageBoxStandard(
            "Exit",
            "Do you want to save before exiting?",
            ButtonEnum.YesNoCancel);
        
        var result = await box.ShowAsPopupAsync(this);

        if (result == ButtonResult.Yes)
        {
            await ViewModel.QuickSaveCommand.ExecuteAsync(null);
            Close();
        }
        else if (result == ButtonResult.No)
        {
            isClosing = true;
            Close();
        }
        else
        {
            //ignore and keep opening
        }
    }
}