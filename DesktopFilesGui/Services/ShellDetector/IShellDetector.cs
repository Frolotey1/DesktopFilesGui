using DesktopFilesGui.Models.Enums;

namespace DesktopFilesGui.Services.ShellDetector;

public interface IShellDetector
{
    public Shell Detect();
}