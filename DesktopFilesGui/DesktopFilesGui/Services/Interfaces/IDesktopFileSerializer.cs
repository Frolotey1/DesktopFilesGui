using DesktopFilesGui.Models;

namespace DesktopFilesGui.Services.Interfaces;

public interface  IDesktopFileSerializer
{
    public string Serialize(DesktopFile desktopFile);
}