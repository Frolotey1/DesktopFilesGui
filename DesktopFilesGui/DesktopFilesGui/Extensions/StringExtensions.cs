using System;

namespace DesktopFilesGui.Extensions;

public static class StringExtensions
{
    public static string AddPostfix(this string value, string postfix)
    {
        ArgumentNullException.ThrowIfNull(value, postfix);

        return value.EndsWith(postfix)
            ? value 
            : value + postfix;
    }
}