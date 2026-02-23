#nullable enable
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using Yohuke.LiveMarker.Models;
using Yohuke.LiveMarker.ViewModels;

namespace Yohuke.LiveMarker.Views.Windows;

public partial class MainWindow : Window
{
    public MainWindowViewModel ViewModel { get; }
    public MainWindow()
    {
        ViewModel = new MainWindowViewModel(this);
        InitializeComponent();
        
        ExtendClientAreaChromeHints = Avalonia.Platform.ExtendClientAreaChromeHints.PreferSystemChrome;
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
        if (isClosing || ViewModel.Data == null || AppRuntime.Settings.EnableAutoSave || !string.IsNullOrWhiteSpace(ViewModel.CurrentFileLocation))
        {
            return;
        }
        
        e.Cancel = true;

        var box = MessageBoxManager.GetMessageBoxStandard(
            AppRuntime.I18N.GetText("Dialog_Exit"),
            AppRuntime.I18N.GetText("Dialog_Exit_Message"),
            ButtonEnum.YesNoCancel);
        
        var result = await box.ShowWindowDialogAsync(this);

        if (result == ButtonResult.Yes)
        {
            isClosing = true;
            await ViewModel.SaveCommand.ExecuteAsync(null);
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

    #region DragMove
    // Solution From: https://github.com/AvaloniaUI/Avalonia/discussions/8441
    private bool _isWindowDragInEffect = false;
    private Point _cursorPositionAtWindowDragStart = new(0, 0);

    private void DragMove_OnPointerMoved(object sender, PointerEventArgs e)
    {
        if (_isWindowDragInEffect)
        {
            var currentCursorPosition = e.GetPosition(this);
            var cursorPositionDelta = currentCursorPosition - _cursorPositionAtWindowDragStart;

            Position = this.PointToScreen(cursorPositionDelta);
        }
    }
    
    private void DragMove_OnPointerPressed(object sender, PointerPressedEventArgs e)
    {
        if (e.Source is Control sourceControl)
        {
            _isWindowDragInEffect = true;
            _cursorPositionAtWindowDragStart = e.GetPosition(this);
        }
    }
    
    private void DragMove_OnPointerReleased(object sender, PointerReleasedEventArgs e)
    {
        _isWindowDragInEffect = false;
    }

    #endregion
}
