using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Yohuke.LiveMarker.Utilities;
using OffsetTimeWindow = Yohuke.LiveMarker.Views.Windows.OffsetTimeWindow;

namespace Yohuke.LiveMarker.ViewModels;

public partial class OffsetTimeWindowViewModel : ViewModelBase<OffsetTimeWindow>
{
    [ObservableProperty] private string offsetInput = string.Empty;
    [ObservableProperty] private bool isInputValid;
    [ObservableProperty] private TimeSpan parsedOffset;

    public bool ShowInvalidHint => !string.IsNullOrWhiteSpace(OffsetInput) && !IsInputValid;

    public OffsetTimeWindowViewModel() { }

    public OffsetTimeWindowViewModel(OffsetTimeWindow window) : base(window) { }

    partial void OnOffsetInputChanged(string value)
    {
        IsInputValid = TimeUtilities.TryParseSignedOffset(value, out var offset);
        if (IsInputValid)
            ParsedOffset = offset;
        OnPropertyChanged(nameof(ShowInvalidHint));
    }

    [RelayCommand]
    private void Confirm()
    {
        View.Close(ParsedOffset);
    }

    [RelayCommand]
    private void Cancel()
    {
        View.Close(null);
    }
}
