using System.Threading.Tasks;
using Yohuke.LiveMarker.Settings;

namespace Yohuke.LiveMarker;

public static class AppRuntime
{
    public static AppSettings Settings { get; private set; }

    public static async Task Init()
    {
        Settings = await AppSettings.Load();
    }
}