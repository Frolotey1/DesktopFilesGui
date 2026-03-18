using System.Collections.Generic;

namespace DesktopFilesGui.Extensions;

public static class EnumerableExtensions
{
    public static string ToDesktopFileArray(this IEnumerable<string> value)
    {
        return string.Join(";", value).AddPostfix(";");
    }
}