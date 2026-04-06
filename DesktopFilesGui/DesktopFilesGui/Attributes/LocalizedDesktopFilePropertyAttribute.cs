using System;
using System.Diagnostics.CodeAnalysis;
using DesktopFilesGui.Models.Enums;

namespace DesktopFilesGui.Attributes;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
[AttributeUsage(AttributeTargets.Property)]
public sealed class LocalizedDesktopFilePropertyAttribute : Attribute, IDesktopFileBaseAttribute
{
    public LocalizedDesktopFilePropertyAttribute(string key, DesktopFileType typeWhenAdd)
    {
        Key = key;
        TypeWhenAdd = typeWhenAdd;
    }
    
    public LocalizedDesktopFilePropertyAttribute (string key)
    {
        Key = key;
        TypeWhenAdd = null;
    }


    public string Key { get; }
    public DesktopFileType? TypeWhenAdd { get; }
}