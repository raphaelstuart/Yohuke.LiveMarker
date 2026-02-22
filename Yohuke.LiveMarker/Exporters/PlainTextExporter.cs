using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Yohuke.LiveMarker.Models;

namespace Yohuke.LiveMarker.Exporters;

public class PlainTextExporter : IMarkerExporter
{
    public async Task Export(LiveMarkerData data, string path)
    {
        var properties = typeof(MarkerData)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanRead)
            .ToArray();

        var lines = new List<string>();

        lines.Add(string.Join("\t", properties.Select(p => p.Name)));

        foreach (var marker in data.Marker.OrderBy(m => m.RealDateTime))
        {
            var values = properties.Select(p =>
            {
                var value = p.GetValue(marker);
                return value?.ToString()?.Replace("\t", " ").Replace("\n", " ").Replace("\r", "") ?? string.Empty;
            });
            lines.Add(string.Join("\t", values));
        }

        await File.WriteAllLinesAsync(path, lines, System.Text.Encoding.UTF8);
    }
}