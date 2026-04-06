using System.Text.Json;
using DesktopFilesGui.JsonSerializerOptions;

namespace DesktopFilesGui.Models;

public sealed class DesktopFileHeader
{
    public required string IconPath { get; set; }
    public required string Name { get; set; }
    public required string Path { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this, DesktopFileHeaderSerializerContext.Default.Options);
    }
}