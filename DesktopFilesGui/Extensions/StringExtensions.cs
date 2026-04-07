using System;
using System.Collections.Generic;
using System.Linq;

namespace DesktopFilesGui.Extensions;

public static class StringExtensions {
    public static string AddPostfix(this string value, string postfix)
    {
        ArgumentNullException.ThrowIfNull(value, postfix);

        return value.EndsWith(postfix)
            ? value 
            : value + postfix;
    }
    
    public static IEnumerable<string> ToEnumerableFromDesktopFileArray(this string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Enumerable.Empty<string>();
        
        return value
            .TrimEnd(';')
            .Split(';', StringSplitOptions.RemoveEmptyEntries)
            .Select(m => m.Trim())
            .Where(m => !string.IsNullOrWhiteSpace(m));
    }
}
