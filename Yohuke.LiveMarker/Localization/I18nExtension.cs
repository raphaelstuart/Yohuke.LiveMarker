using System;
using Avalonia;
using Avalonia.Markup.Xaml;

namespace Yohuke.LiveMarker.I18n;

public class I18nExtension : MarkupExtension
{
    public string Key { get; set; }

    public I18nExtension() { }

    public I18nExtension(string key)
    {
        Key = key;
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var observable = new LocalizationObservable(Key);
        return observable.ToBinding();
    }
}