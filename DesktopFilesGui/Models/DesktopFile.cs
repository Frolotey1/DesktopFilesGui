using System.Collections.Generic;
using DesktopFilesGui.Models.Enums;

namespace DesktopFilesGui.Models;

public sealed class DesktopFile
{
    public DesktopFileType Type { get; set; }
    public bool ShowTerminal { get; set; }
    public bool RequireSudo { get; set; }
    public bool DisplayInMenu { get; set; }
    public bool EnableLocalization { get; set; }
    public bool IsHidden { get; set; }
    public bool RunFromDBus { get; set; }
    public bool StartupNotifySupport { get; set; }
    public bool UseCustomExecCommand { get; set; }
    public IEnumerable<string> SupportedMimeTypes { get; set; }
}