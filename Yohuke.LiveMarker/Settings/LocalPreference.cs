using System;
using System.IO;
using Newtonsoft.Json;
using Yohuke.LiveMarker.Utilities;

namespace Yohuke.LiveMarker.Settings;

public class LocalPreference
{
    public AppSettings AppSettings { get; set; }
    
    public void SaveLauncherSettings()
    {
        File.WriteAllText(PathUtilities.ConfigFile, JsonConvert.SerializeObject(AppSettings));
    }
    
    private T LoadSettingsInternal<T>(T defaultValue)
    {
        var path = PathUtilities.ConfigFile;
  
        if (!File.Exists(path))
        {
            return defaultValue;
        }

        try
        {
            var data = File.ReadAllText(path);
            var settings = JsonConvert.DeserializeObject<T>(data);
            return settings;
        }
        catch (Exception e)
        {
            return defaultValue;
        }
    }

    public void LoadLauncherSettings()
    {
        var path = new DirectoryInfo("Lumin");

        if (!path.Exists)
        {
            path.Create();
        }
        
        AppSettings = LoadSettingsInternal(
            new AppSettings
            {
               
            });
        
        SaveLauncherSettings();
    }
}