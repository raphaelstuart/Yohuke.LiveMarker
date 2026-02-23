using System;

namespace Yohuke.LiveMarker.I18n;

public class LocalizationObservable(string key) : IObservable<object>
{
    public IDisposable Subscribe(IObserver<object> observer)
    {
        observer.OnNext(AppRuntime.I18N.GetText(key));

        void Handler(string _)
        {
            observer.OnNext(AppRuntime.I18N.GetText(key));
        }
        
        AppRuntime.I18N.LanguageChanged += Handler;

        return new LocalizationUnsubscriber(() => AppRuntime.I18N.LanguageChanged -= Handler);
    }
}