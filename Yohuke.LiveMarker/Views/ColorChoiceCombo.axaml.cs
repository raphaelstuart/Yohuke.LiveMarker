using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Media;
using Yohuke.LiveMarker.Models;
using Yohuke.LiveMarker.Utilities;

namespace Yohuke.LiveMarker.Views;

public partial class ColorChoiceCombo : UserControl
{
    public static readonly StyledProperty<bool> ShowColorNameProperty =
        AvaloniaProperty.Register<ColorChoiceCombo, bool>(
            nameof(ShowColorName), defaultBindingMode: BindingMode.TwoWay);
    
    public static readonly StyledProperty<MarkerColorDefinition> SelectedColorProperty =
        AvaloniaProperty.Register<ColorChoiceCombo, MarkerColorDefinition>(
            nameof(SelectedColor), defaultBindingMode: BindingMode.TwoWay);

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