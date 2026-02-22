using System;

namespace Yohuke.LiveMarker.Utilities;

public static class TimeUtilities
{
    public static DateTime TruncateMilliseconds(this DateTime dt)
    {
        return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
    }
}