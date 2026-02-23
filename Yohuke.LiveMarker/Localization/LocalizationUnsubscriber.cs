using System;

namespace Yohuke.LiveMarker.I18n;

public class LocalizationUnsubscriber(Action unsubscribe) : IDisposable
{
    public void Dispose()
    {
        unsubscribe();
    }
}