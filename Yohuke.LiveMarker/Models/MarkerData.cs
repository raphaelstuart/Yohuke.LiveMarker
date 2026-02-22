using System;
using CommunityToolkit.Mvvm.ComponentModel;
using YamlDotNet.Serialization;

namespace Yohuke.LiveMarker.Models;

public partial class MarkerData : ObservableObject
{
    [ObservableProperty]
    [property: YamlMember]
    private DateTime realDateTime;
    
    [ObservableProperty]
    [property: YamlMember]
    private TimeSpan liveTime;
    
    [ObservableProperty] 
    [property: YamlMember]
    private string message;

    [ObservableProperty]
    [property: YamlMember]
    private MarkerColor markerColor;

    partial void OnRealDateTimeChanged(DateTime oldValue, DateTime newValue)
    {
        LiveTime += newValue - oldValue;
    }
    
    public MarkerData CreateSnapshot()
    {
        return new MarkerData
        {
            Message = Message,
            RealDateTime = RealDateTime,
            MarkerColor = MarkerColor,
            LiveTime = LiveTime
        };
    }
    
    public bool IsEqual(MarkerData other)
    {
        if (other == null) return false;
        return Message == other.Message
               && RealDateTime == other.RealDateTime
               && MarkerColor == other.MarkerColor
               && LiveTime == other.LiveTime;
    }
}