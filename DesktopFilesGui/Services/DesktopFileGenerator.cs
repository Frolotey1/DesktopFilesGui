using System.Linq;
using System.Text;
using DesktopFilesGui.Extensions;
using DesktopFilesGui.Models;
using DesktopFilesGui.Services.Interfaces;
using Serilog;

namespace DesktopFilesGui.Services;

public class DesktopFileGenerator(ILogger logger) : IDesktopFileGenerator
{
    public string Generate(DesktopFile desktopFile)
    {
        var fileContentBuilder = new StringBuilder();
        
        logger.Debug($"Generating desktop file from: {desktopFile}...");
        
        fileContentBuilder
            .AppendLine(Configuration.DESKTOP_FILE_STARTING)
            .AppendLine($"{Configuration.TYPE_KEY}={desktopFile.Type}")
            .AppendLine($"{Configuration.TERMINAL_KEY}={desktopFile.ShowTerminal
                .ToString()
                .ToLower()}")
            .AppendLine($"{Configuration.NO_DISPLAY_KEY}={(!desktopFile.DisplayInMenu)
                .ToString()
                .ToLower()}")
            .AppendLine($"{Configuration.HIDDEN_KEY}={desktopFile.IsHidden
                .ToString()
                .ToLower()}")
            .AppendLine($"{Configuration.DBUS_ACTIVATABLE_KEY}={desktopFile.RunFromDBus
                .ToString()
                .ToLower()}")
            .AppendLine($"{Configuration.STARTUP_NOTIFY_KEY}={desktopFile.StartupNotifySupport
                .ToString()
                .ToLower()}");
        
        if(desktopFile.SupportedMimeTypes.Any())
            fileContentBuilder
                .AppendLine($"{Configuration.MIME_TYPE_KEY}={desktopFile.SupportedMimeTypes
                    .ToDesktopFileArray()}");
        
        var result = fileContentBuilder.ToString();
        logger.Information("Generated .desktop file \n {code}", result);
        return result;
    }
}