using System.Threading.Tasks;
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
    
    private async void MarkerDataGrid_OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Delete && sender is DataGrid { SelectedItem: MarkerData marker })
        {
            await ViewModel.DeleteMarkerCommand.ExecuteAsync(marker);
            e.Handled = true;
        }
    }

    private bool isClosing;
    protected override async void OnClosing(WindowClosingEventArgs e)
    {
        if (isClosing || !string.IsNullOrWhiteSpace(ViewModel.CurrentFileLocation))
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
            isClosing = true;
            await ViewModel.QuickSaveCommand.ExecuteAsync(null);
            Close();
        }
        else if (result == ButtonResult.No)
        {
            isClosing = true;
            Close();
        }
    }

    private void DataGrid_OnBeginningEdit(object sender, DataGridBeginningEditEventArgs e)
    {
        if (e.Row.DataContext is MarkerData marker)
        {
            ViewModel.BeginEditMarker(marker);
        }
    }

    private void DataGrid_OnCellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
    {
        if (e.EditAction == DataGridEditAction.Commit)
        {
            ViewModel.CommitEditMarker();
        }
        else
        {
            ViewModel.EndEditMarker();
        }
    }
}
