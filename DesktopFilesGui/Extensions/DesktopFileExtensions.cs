using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DesktopFilesGui.Attributes;
using DesktopFilesGui.Models;

namespace DesktopFilesGui.Extensions;

public static class DesktopFileExtensions
{
    public static IEnumerable<DesktopFileProperty> GetDesktopFileProperties<TAttribute>(this DesktopFile desktopFile) where TAttribute : Attribute, IDesktopFileBaseAttribute
    {
        return desktopFile.GetType()
            .GetProperties()
            .Select(property =>
                new DesktopFileProperty(property.GetCustomAttribute<TAttribute>(), property))
            .Where(prop => prop.Attribute is not null)
            .Where(prop =>
                prop.Attribute!.TypeWhenAdd is null
                || prop.Attribute!.TypeWhenAdd == desktopFile.Type);
    }
}