using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Yohuke.LiveMarker.Utilities;

namespace Yohuke.LiveMarker.Settings;

public class LocalCache
{
    private Dictionary<string, string> cache;
    
    public LocalCache()
    {
        try
        {
            var data = File.ReadAllText(PathUtilities.LocalCachePath);
            cache = JsonConvert.DeserializeObject<Dictionary<string, string>>(data);
        }
        catch
        {
            cache = new();
            Save();
        }
    }
    
    public T GetObject<T>(string key, T defaultValue = default) where T: new()
    {
        try
        {
            if (cache.TryGetValue(key, out var val))
            {
                return JsonConvert.DeserializeObject<T>(val);
            }
        }
        catch (Exception e)
        {
            // ignored
        }
        
        return defaultValue;
    }

    public string GetString(string key, string defaultValue = null)
    {
        if (cache.TryGetValue(key, out var val))
        {
            return val;
        }

        return defaultValue;
    }

    public float GetFloat(string key, float defaultValue = 0)
    {
        if (cache.TryGetValue(key, out var val))
        {
            return float.Parse(val);
        }

        return defaultValue;
    }

    public int GetInt(string key, int defaultValue = 0)
    {
        if (cache.TryGetValue(key, out var val))
        {
            return int.Parse(val);
        }

        return defaultValue;
    }
    
    public long GetLong(string key, long defaultValue = 0)
    {
        if (cache.TryGetValue(key, out var val))
        {
            return long.Parse(val);
        }

        return defaultValue;
    }

    public bool GetBool(string key, bool defaultValue = false)
    {
        if (cache.TryGetValue(key, out var val))
        {
            return bool.Parse(val);
        }

        return defaultValue;
    }

    public void SetString(string key, string value)
    {
        cache[key] = value;
        Save();
    }
    
    public void SetObject<T>(string key, T value) where T: new()
    {
        cache[key] = JsonConvert.SerializeObject(value);
        Save();
    }

    public void SetFloat(string key, float value)
    {
        cache[key] = value.ToString();
        Save();
    }

    public void SetInt(string key, int value)
    {
        cache[key] = value.ToString();
        Save();
    }
    
    public void SetLong(string key, long value)
    {
        cache[key] = value.ToString();
        Save();
    }

    public void SetBool(string key, bool value)
    {
        cache[key] = value.ToString();
        Save();
    }

    public void Save()
    {
        File.WriteAllText(PathUtilities.LocalCachePath, JsonConvert.SerializeObject(cache));
    }
}
