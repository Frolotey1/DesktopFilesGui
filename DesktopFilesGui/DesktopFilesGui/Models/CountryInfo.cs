
using System.Collections.Generic;

namespace DesktopFilesGui.Models;

public sealed class CountryInfo
{
    public required string IconPath { get; set; }
    public required List<string> Keys { get; set; }
    public required string CountryName { get; set; }
}