using System.Threading.Tasks;
using Yohuke.LiveMarker.Models;

namespace Yohuke.LiveMarker.Exporters;

public interface IMarkerExporter
{
    Task Export(LiveMarkerData data, string path);
}