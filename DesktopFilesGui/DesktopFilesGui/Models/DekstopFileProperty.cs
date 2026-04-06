using System;
using System.Reflection;
using DesktopFilesGui.Attributes;

namespace DesktopFilesGui.Models;

public record DesktopFileProperty(IDesktopFileBaseAttribute? Attribute, object ValueActual);