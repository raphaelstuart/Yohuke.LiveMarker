namespace Yohuke.LiveMarker.Models;

public record LanguageOption(string Code, string DisplayName)
{
    public override string ToString() => DisplayName;
}