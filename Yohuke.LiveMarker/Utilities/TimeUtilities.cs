using System;
using System.Text.RegularExpressions;

namespace Yohuke.LiveMarker.Utilities;

public static class TimeUtilities
{
    public static DateTime TruncateMilliseconds(this DateTime dt)
    {
        return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
    }

    // Matches time patterns: hh:mm:ss, hh.mm.ss, hh：mm：ss, hh-mm-ss (full-width and half-width)
    private static readonly Regex TimePattern = new(
        @"^(\d{1,2})[:\.\-：．\uff0d](\d{1,2})[:\.\-：．\uff0d](\d{1,2})",
        RegexOptions.Compiled);

    /// <summary>
    /// Tries to parse a flexible time format from the beginning of the input string.
    /// Accepts hh:mm:ss, hh.mm.ss, hh：mm：ss, hh-mm-ss.
    /// Returns true if a time was found, with the parsed TimeSpan and the remaining text.
    /// </summary>
    public static bool TryParseFlexibleTime(string input, out TimeSpan time, out string remaining)
    {
        time = TimeSpan.Zero;
        remaining = input;

        if (string.IsNullOrWhiteSpace(input))
            return false;

        var match = TimePattern.Match(input);
        if (!match.Success)
            return false;

        if (int.TryParse(match.Groups[1].Value, out var h) &&
            int.TryParse(match.Groups[2].Value, out var m) &&
            int.TryParse(match.Groups[3].Value, out var s) &&
            h is >= 0 and < 24 && m is >= 0 and < 60 && s is >= 0 and < 60)
        {
            time = new TimeSpan(h, m, s);
            remaining = input[match.Length..].TrimStart();
            return true;
        }

        return false;
    }
}