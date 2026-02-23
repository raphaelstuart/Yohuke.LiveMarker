using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Yohuke.LiveMarker.I18n;

public class Localization : INotifyPropertyChanged
{
    private Dictionary<string, string> currentStrings;
    private Dictionary<string, string> fallbackStrings;
    private string currentLang = "en";

    public event Action<string> LanguageChanged;

    public event PropertyChangedEventHandler PropertyChanged;
    public string CurrentLang => currentLang;

    public string this[string key] => GetText(key);

    public Localization()
    {
        fallbackStrings = LoadLanguageFile("en");
        currentStrings = fallbackStrings;
    }

    public string GetText(string key)
    {
        return currentStrings.TryGetValue(key, out var value)
            ? value
            : fallbackStrings.GetValueOrDefault(key, key);
    }

    public void SetLang(string langName)
    {
        if (string.IsNullOrWhiteSpace(langName))
        {
            langName = "en";
        }

        currentLang = langName;
        currentStrings = LoadLanguageFile(langName);

        if (currentLang != "en")
        {
            fallbackStrings = LoadLanguageFile("en");
        }

        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Item[]"));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentLang)));
        LanguageChanged?.Invoke(currentLang);
    }

    private static Dictionary<string, string> LoadLanguageFile(string langName)
    {
        try
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"Yohuke.LiveMarker.I18n.{langName}.yml";

            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream != null)
            {
                using var reader = new StreamReader(stream);
                var deserializer = new DeserializerBuilder()
                    .WithNamingConvention(NullNamingConvention.Instance)
                    .Build();
                return deserializer.Deserialize<Dictionary<string, string>>(reader) ?? new();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load language '{langName}': {ex.Message}");
        }

        return new Dictionary<string, string>();
    }
}
