using System;
using System.IO;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using YamlDotNet.Serialization;
using Yohuke.LiveMarker.Utilities;

namespace Yohuke.LiveMarker.Settings;

public partial class AppSettings : ObservableObject
{
    [ObservableProperty]
    [field:YamlMember]
    private bool enableAutoSave = true;
    
    [ObservableProperty]
    [field:YamlMember]
    private bool showDateTimeColumn = true;
    
    [ObservableProperty]
    [field:YamlMember]
    private string language = "en";
    
    public static async Task<AppSettings> Load()
    {
        try
        {
            if (File.Exists(PathUtilities.ConfigFile))
            {
                var data = await File.ReadAllTextAsync(PathUtilities.ConfigFile);
                return new Deserializer().Deserialize<AppSettings>(data);
            }

            throw new FileNotFoundException();
        }
        catch (Exception)
        {
            var nd = new AppSettings();
            await nd.Save();
            return nd;
        }
    }

    public async Task Save()
    {
        try
        {
            var data = new Serializer().Serialize(this);
            await File.WriteAllTextAsync(PathUtilities.ConfigFile, data);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}