using System.Collections.Generic;

namespace DesktopFilesGui.Models;

public class Configuration
{
    public required ICollection<ExecutionTemplates> ExecutionTemplatesCollection { get; set; }
}