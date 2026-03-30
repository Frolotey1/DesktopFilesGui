using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using DesktopFilesGui.Attributes;
using DesktopFilesGui.Models.Enums;

namespace DesktopFilesGui.Models;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
public sealed class DesktopFile
{
    [DesktopFileProperty("Type")]
    public DesktopFileType Type { get; set; }
    
    [DesktopFileProperty("Terminal",  DesktopFileType.Application)]
    public bool ShowTerminal { get; set; }
    public bool RequireSudo { get; set; }
    
    [DesktopFileProperty("Terminal", DesktopFileType.Application)]
    public bool HiddenInMenu { get; set; }
    public bool EnableLocalization { get; set; }
    
    [DesktopFileProperty("Hidden")]
    public bool IsHidden { get; set; }
    
    [DesktopFileProperty("DBusActivatable", DesktopFileType.Application)]
    public bool RunFromDBus { get; set; }
    
    [DesktopFileProperty("StartupNotify", DesktopFileType.Application)]
    public bool StartupNotifySupport { get; set; }
    
    public string? CustomExecCommand { get; set; }
    public string? CustomTryExecCommand { get; set; }
    
    [DesktopFileProperty("Path", DesktopFileType.Application)]
    public string? Path { get; set; }
    
    [DesktopFileProperty("MimeType", DesktopFileType.Application)]
    public IEnumerable<string> SupportedMimeTypes { get; set; } = [];
    
    [DesktopFileProperty("Name")]
    public string? Name { get; set; }
    
    [LocalizedDesktopFileProperty("Name")]
    public IDictionary<string, string>? LocalizedName { get; set; }
    
    [DesktopFileProperty("GenericName")]
    public string? GenericName { get; set; }
    
    [LocalizedDesktopFileProperty("GenericName")]
    public IDictionary<string, string>? LocalizedGenericName { get; set; }
    
    [DesktopFileProperty("Comment")]
    public string? Comment { get; set; }
    
    [LocalizedDesktopFileProperty("Comment")]
    public IDictionary<string, string>? LocalizedComment { get; set; }
}