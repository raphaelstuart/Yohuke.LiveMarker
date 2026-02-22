using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using YamlDotNet.Serialization;
using Yohuke.LiveMarker.Utilities;

namespace Yohuke.LiveMarker.Models;

public partial class LiveMarkerData : ObservableObject
{
    [ObservableProperty]
    [property: YamlMember]
    private DateTime startTime = DateTime.Now.TruncateMilliseconds();

    [ObservableProperty] [property: YamlMember]
    private ObservableCollection<MarkerData> marker = new();
    
    public void CalculateLiveTime()
    {
        foreach (var markerData in Marker)
        {
            markerData.RealDateTime = StartTime + markerData.LiveTime;
        }
    }

    partial void OnStartTimeChanged(DateTime value)
    {
        CalculateLiveTime();
    }
    
    public static async Task<LiveMarkerData> Load(string path)
    {
        try
        {
            var content = await File.ReadAllTextAsync(path);
            var deserializer = new DeserializerBuilder().Build();
            var data = deserializer.Deserialize<LiveMarkerData>(content);
            data.CalculateLiveTime();

            return data;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
    
    public async Task Save(string path)
    {
        try
        {
            CalculateLiveTime();
            var serializer = new SerializerBuilder().Build();
            var yaml = serializer.Serialize(this);
            await File.WriteAllTextAsync(path, yaml);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}