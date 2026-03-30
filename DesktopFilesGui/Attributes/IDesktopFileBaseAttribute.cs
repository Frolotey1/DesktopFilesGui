using DesktopFilesGui.Models.Enums;

namespace DesktopFilesGui.Attributes;

public interface IDesktopFileBaseAttribute
{
    public string Key { get; }
    public DesktopFileType? TypeWhenAdd { get; }
}