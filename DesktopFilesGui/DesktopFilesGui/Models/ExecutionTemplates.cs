namespace DesktopFilesGui.Models;

public sealed class ExecutionTemplates
{
    public required string Template { get; set; }
    public required string Exec { get; set; }
    public string? TryExec { get; set; }
}