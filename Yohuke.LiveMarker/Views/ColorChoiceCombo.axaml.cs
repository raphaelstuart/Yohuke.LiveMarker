using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Media;
using Yohuke.LiveMarker.Models;

namespace Yohuke.LiveMarker.Views;

public partial class ColorChoiceCombo : UserControl
{
    public static readonly StyledProperty<MarkerColorDefinition> SelectedColorProperty =
        AvaloniaProperty.Register<ColorChoiceCombo, MarkerColorDefinition>(
            nameof(SelectedColor), defaultBindingMode: BindingMode.TwoWay);
    
    public static readonly StyledProperty<bool> ShowColorNameProperty =
        AvaloniaProperty.Register<ColorChoiceCombo, bool>(
            nameof(ShowColorName), defaultBindingMode: BindingMode.TwoWay);

    private static readonly ObservableCollection<MarkerColorDefinition> _colorChoices = new()
    {
        new (MarkerColor.Red, Brushes.Red),
        new (MarkerColor.Orange, Brushes.Orange),
        new (MarkerColor.Yellow, Brushes.Yellow),
        new (MarkerColor.Green, Brushes.Green),
        new (MarkerColor.Blue, Brushes.Blue),
        new (MarkerColor.Magenta, Brushes.Magenta),
        new (MarkerColor.Grey, Brushes.Gray)
    };

    public static ObservableCollection<MarkerColorDefinition> ColorChoices => _colorChoices;

    public bool ShowColorName
    {
        get => GetValue(ShowColorNameProperty);
        set => SetValue(ShowColorNameProperty, value);
    }
    
    public MarkerColorDefinition SelectedColor
    {
        get => GetValue(SelectedColorProperty);
        set => SetValue(SelectedColorProperty, value);
    }

    public ColorChoiceCombo()
    {
        InitializeComponent();
    }
}