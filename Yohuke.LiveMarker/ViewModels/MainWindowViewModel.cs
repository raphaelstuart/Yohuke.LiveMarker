using System;
using System.Collections.ObjectModel;
using System.Linq;
using Yohuke.LiveMarker.Models;
using Yohuke.LiveMarker.Views;

namespace Yohuke.LiveMarker.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<MarkerData> Data { get; set; } = new();

    public MarkerColorDefinition CurrentSelectedColor { get; set; } = ColorChoiceCombo.ColorChoices[3];
    public string CurrentInputMessage { get; set; }
    public DateTime InputTime { get; set; } = DateTime.Now;
    
    public MainWindowViewModel()
    {
        CurrentInputMessage = "Hello";
        Data.Add(new MarkerData
        {
            RealDateTime = DateTime.Now,
            LiveTime = TimeSpan.FromSeconds(2421),
            Message = "Hello, World!",
            MarkerColor = ColorChoiceCombo.ColorChoices.FirstOrDefault()
        });
    }
}