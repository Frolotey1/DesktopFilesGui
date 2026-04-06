using System.Collections.Generic;
using DesktopFilesGui.Models;

namespace DesktopFilesGui.Extensions;

public static class EnumerableExtensions
{
    public static string ToDesktopFileArray(this IEnumerable<string> value)
    {
        return string.Join(";", value).AddPostfix(";");
    }
}