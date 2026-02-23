using System;
using System.IO;

namespace Yohuke.LiveMarker.Utilities;

public static class PathUtilities
{
    public static string AppDataDir 
    {
        get
        {
            var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "yosymph", "LiveMarker");

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return dir;
        }
    }
    
        
    public static string LocalCachePath
    {
        get
        {
            var file = Path.Combine(AppDataDir, "cache.json");

            if (!File.Exists(file))
            {
                File.WriteAllText(file, "{}");
            }

            return file;
        }
    }
    
    public static string ConfigFile => Path.Combine(AppDataDir, "config.json");
}