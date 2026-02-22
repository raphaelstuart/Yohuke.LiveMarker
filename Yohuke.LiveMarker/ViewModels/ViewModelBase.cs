using System.ComponentModel;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Yohuke.LiveMarker.ViewModels;

public abstract class ViewModelBase : ObservableObject
{
    public ViewModelBase() {}
}

public abstract class ViewModelBase<T> : ViewModelBase where T : Window
{
    public T View { get; set; }

    public ViewModelBase(T window)
    {
        window.DataContext = this;
        View = window;
    }

    public ViewModelBase() {}
}