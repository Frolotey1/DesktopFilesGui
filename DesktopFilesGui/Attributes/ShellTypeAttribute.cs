using System;
using DesktopFilesGui.Models.Enums;

namespace DesktopFilesGui.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class ShellTypeAttribute(Shell shell) : Attribute
{
    public Shell Shell { get; } = shell;
}