using System;
using System.Diagnostics.CodeAnalysis;
using DesktopFilesGui.Models.Enums;

namespace DesktopFilesGui.Attributes;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
[AttributeUsage(AttributeTargets.Property)]
public sealed class DesktopFilePropertyAttribute : Attribute, IDesktopFileBaseAttribute
{
    public DesktopFilePropertyAttribute(string key, DesktopFileType typeWhenAdd)
    {
        Key = key;
        TypeWhenAdd = typeWhenAdd;
    }
    
    public DesktopFilePropertyAttribute(string key)
    {
        Key = key;
        TypeWhenAdd = null;
    }


    public string Key { get; }
    public DesktopFileType? TypeWhenAdd { get; }
}