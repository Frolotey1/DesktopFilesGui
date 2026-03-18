using System;
using System.Collections.Generic;
using System.IO;
using DesktopFilesGui.Models;
using DesktopFilesGui.Models.Enums;

namespace DesktopFilesGui;

public static class Configuration
{
    public const string SERILOG_OUTPUT_TEMPLATE = "[{Timestamp:HH:mm:ss} {Level}] [Thread: {ThreadId}] {Message:lj}{NewLine}{Exception}";
    public static readonly string APPLICATION_DATA = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DesktopFilesGui");
    public const string GITHUB_LINK = "https://github.com/TheMakarik/DesktopFilesGui";
    public const string DEFAULT_DESKTOP_FILE_PATH = "/usr/share/applications";
    
    public const DesktopFileType DEFAULT_DESKTOP_FILE_TYPE = DesktopFileType.Application;
    public const string TYPE_KEY = "Type";
    public const string TERMINAL_KEY = "Terminal";
    public const string NO_DISPLAY_KEY = "NoDisplay";
    public const string HIDDEN_KEY = "Hidden";
    public const string DBUS_ACTIVATABLE_KEY = "DBusActivatable";
    public const string PATH_IN_EXEC_COMMAND = "{PATH}";
    public const string STARTUP_NOTIFY_KEY = "StartupNotify";
    public const string MIME_TYPE_KEY = "MimeType";
    public static readonly string EXECUTION_TEMPLATES_JSON_PATH = $"{APPLICATION_DATA}/triggers.json";

    public static readonly TemplatesInfo DEFAULT_TEMPLATES_INFO = new()
    {
        ExecutionTemplatesCollection =
        [
            new ExecutionTemplates()
            {
                Template = "*.sh",
                Exec = $"sh {PATH_IN_EXEC_COMMAND}"
            },
            new ExecutionTemplates()
            {
                 Template = "*.bash",
                 Exec = $"bash {PATH_IN_EXEC_COMMAND}"
            },
            new ExecutionTemplates()
            {
                Template = "*.run",
                Exec = $"{PATH_IN_EXEC_COMMAND}"
            },
            new ExecutionTemplates()
            {
                Template = "*.AppImage",
                Exec = $"{PATH_IN_EXEC_COMMAND}"
            },
        ]
    };
    
    public const string DESKTOP_FILE_STARTING = @$"
# This .desktop file powered by {nameof(DesktopFilesGui)}
# It's extreamly useful application to create desktop files     
# See more: {GITHUB_LINK}

[Desktop Entry]";
    
    public static List<CountryInfo> Countries { get; } =
    [
        new() { IconPath = "/Assets/Flags/USA.png", Key = "us" },
        new() { IconPath = "/Assets/Flags/brazil.png", Key = "br" },
        new() { IconPath = "/Assets/Flags/canada.png", Key = "ca" },
        new() { IconPath = "/Assets/Flags/china.png", Key = "cn" },
        new() { IconPath = "/Assets/Flags/france.png", Key = "fr" },
        new() { IconPath = "/Assets/Flags/germany.png", Key = "de" },
        new() { IconPath = "/Assets/Flags/japan.png", Key = "jp" },
        new() { IconPath = "/Assets/Flags/norway.png", Key = "no" },
        new() { IconPath = "/Assets/Flags/russia.png", Key = "ru" },
        new() { IconPath = "/Assets/Flags/uk.png", Key = "gb" },
        new() { IconPath = "/Assets/Flags/ireland (1).png", Key = "ie" },
        new() { IconPath = "/Assets/Flags/italy.png", Key = "it" },
        new() { IconPath = "/Assets/Flags/korea.png", Key = "kr" }
    ];
}
