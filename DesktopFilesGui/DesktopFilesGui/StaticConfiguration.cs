using System;
using System.Collections.Generic;
using System.IO;
using DesktopFilesGui.Models;
using DesktopFilesGui.Models.Enums;

namespace DesktopFilesGui;

public static class StaticConfiguration
{
    public const string SERILOG_OUTPUT_TEMPLATE = "[{Timestamp:HH:mm:ss} {Level}] [Thread: {ThreadId}] {Message:lj}{NewLine}{Exception}";
    public static readonly string APPLICATION_DATA = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DesktopFilesGui");
    public const string GITHUB_LINK = "https://github.com/TheMakarik/DesktopFilesGui";
    public const string DEFAULT_DESKTOP_FILE_PATH = "/usr/share/applications";
    public const string DESKTOP_FILE_EXTENSION = ".desktop";
    
    public const DesktopFileType DEFAULT_DESKTOP_FILE_TYPE = DesktopFileType.Application;
    public const string PATH_IN_EXEC_COMMAND = "{PATH}";
    public static readonly string CONFIGURATION_JSON_PATH = $"{APPLICATION_DATA}/config.json";

    public static readonly Configuration DEFAULT_CONFIGURATION = new()
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
    
}
