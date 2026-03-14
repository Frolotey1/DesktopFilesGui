namespace DesktopFilesGui.Models;

public sealed class ShellTheme
{
    public bool IsUnknown { get; private set; }
    
    public ShellColor Primary { get;  set; }
    public ShellColor Secondary { get; set; }
 
    public static ShellTheme Unknown => new ShellTheme(){IsUnknown =  true};
}