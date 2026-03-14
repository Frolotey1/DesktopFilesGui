using System;
using System.Collections.Generic;
using System.IO;
using DesktopFilesGui.Models;

namespace DesktopFilesGui;

public static class Configuration
{
    public const string SERILOG_OUTPUT_TEMPLATE = "[{Timestamp:HH:mm:ss} {Level}] [Thread: {Thread}] {Message:lj}{NewLine}{Exception}";
    public static readonly string APPLICATION_DATA = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DesktopFilesGui");
    public const string GITHUB_LINK = "https://github.com/TheMakarik/DesktopFilesGui";
    
    
    public const string DesktopFileStarting = @$"
# This .desktop file powered by {nameof(DesktopFilesGui)}
# It's extreamly useful application to create desktop files
# See more: {GITHUB_LINK}

[Desktop Entry]
";
    
    public static List<CountryInfo> Countries { get; } = new()
    {
        new CountryInfo { IconPath = "/Assets/Flags/USA.png", Key = "us" },
        new CountryInfo { IconPath = "/Assets/Flags/brazil.png", Key = "br" },
        new CountryInfo { IconPath = "/Assets/Flags/canada.png", Key = "ca" },
        new CountryInfo { IconPath = "/Assets/Flags/china.png", Key = "cn" },
        new CountryInfo { IconPath = "/Assets/Flags/france.png", Key = "fr" },
        new CountryInfo { IconPath = "/Assets/Flags/germany.png", Key = "de" },
        new CountryInfo { IconPath = "/Assets/Flags/japan.png", Key = "jp" },
        new CountryInfo { IconPath = "/Assets/Flags/norway.png", Key = "no" },
        new CountryInfo { IconPath = "/Assets/Flags/russia.png", Key = "ru" },
        new CountryInfo { IconPath = "/Assets/Flags/uk.png", Key = "gb" },
        new CountryInfo { IconPath = "/Assets/Flags/ireland (1).png", Key = "ie" },
        new CountryInfo { IconPath = "/Assets/Flags/italy.png", Key = "it" },
        new CountryInfo { IconPath = "/Assets/Flags/korea.png", Key = "kr" }
    };
}
