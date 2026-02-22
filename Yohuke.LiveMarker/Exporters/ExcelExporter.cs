using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Yohuke.LiveMarker.Models;
using SixLabors.ImageSharp.PixelFormats;

namespace Yohuke.LiveMarker.Exporters;

public class ExcelExporter : IMarkerExporter
{
    private static Rgb24 GetRgbForMarkerColor(MarkerColor color) => color switch
    {
        MarkerColor.Red     => new Rgb24(0xFF, 0x94, 0x80),
        MarkerColor.Orange  => new Rgb24(0xFF, 0xD5, 0x99),
        MarkerColor.Yellow  => new Rgb24(0xFF, 0xFF, 0x99),
        MarkerColor.Green   => new Rgb24(0xBB, 0xFF, 0x77),
        MarkerColor.Blue    => new Rgb24(0x99, 0xD1, 0xFF),
        MarkerColor.Magenta => new Rgb24(0xEE, 0xBB, 0xFF),
        MarkerColor.Grey    => new Rgb24(0xCC, 0xCC, 0xCC),
        _                   => new Rgb24(0xFF, 0xFF, 0xFF),
    };

    private static XSSFColor ToXSSFColor(Rgb24 rgb) => new(rgb);

    public Task Export(LiveMarkerData data, string path)
    {
        return Task.Run(() =>
        {
            var properties = typeof(MarkerData)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead)
                .ToArray();

            var workbook = new XSSFWorkbook();
            var sheet = workbook.CreateSheet("Markers");

            var headerStyle = workbook.CreateCellStyle();
            var headerFont = workbook.CreateFont();
            headerFont.IsBold = true;
            headerStyle.SetFont(headerFont);
            ((XSSFCellStyle)headerStyle).SetFillForegroundColor(ToXSSFColor(new Rgb24(0xBD, 0xD7, 0xEE)));
            headerStyle.FillPattern = FillPattern.SolidForeground;

            var headerRow = sheet.CreateRow(0);
            for (var col = 0; col < properties.Length; col++)
            {
                var cell = headerRow.CreateCell(col);
                cell.SetCellValue(properties[col].Name);
                cell.CellStyle = headerStyle;
            }

            var styleCache = new Dictionary<MarkerColor, ICellStyle>();
            ICellStyle GetRowStyle(MarkerColor markerColor)
            {
                if (styleCache.TryGetValue(markerColor, out var cached))
                    return cached;

                var style = workbook.CreateCellStyle();
                ((XSSFCellStyle)style).SetFillForegroundColor(
                    ToXSSFColor(GetRgbForMarkerColor(markerColor)));
                style.FillPattern = FillPattern.SolidForeground;
                styleCache[markerColor] = style;
                return style;
            }

            var ordered = data.Marker.OrderBy(m => m.RealDateTime).ToList();
            for (var row = 0; row < ordered.Count; row++)
            {
                var dataRow = sheet.CreateRow(row + 1);
                var marker = ordered[row];
                var rowStyle = GetRowStyle(marker.MarkerColor);

                for (var col = 0; col < properties.Length; col++)
                {
                    var cell = dataRow.CreateCell(col);
                    cell.CellStyle = rowStyle;

                    var value = properties[col].GetValue(marker);
                    switch (value)
                    {
                        case null:
                            cell.SetCellValue(string.Empty);
                            break;
                        case bool b:
                            cell.SetCellValue(b);
                            break;
                        case int i:
                            cell.SetCellValue(i);
                            break;
                        case long l:
                            cell.SetCellValue(l);
                            break;
                        case double d:
                            cell.SetCellValue(d);
                            break;
                        case float f:
                            cell.SetCellValue(f);
                            break;
                        case System.DateTime dt:
                            cell.SetCellValue(dt.ToString("yyyy-MM-dd HH:mm:ss"));
                            break;
                        case System.TimeSpan ts:
                            cell.SetCellValue(ts.ToString(@"hh\:mm\:ss\.fff"));
                            break;
                        case MarkerColor mc:
                            cell.SetCellValue(mc.ToString());
                            break;
                        default:
                            cell.SetCellValue(value.ToString() ?? string.Empty);
                            break;
                    }
                }
            }

            for (var col = 0; col < properties.Length; col++)
            {
                sheet.AutoSizeColumn(col);
            }

            using var fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            workbook.Write(fs);
        });
    }
}
