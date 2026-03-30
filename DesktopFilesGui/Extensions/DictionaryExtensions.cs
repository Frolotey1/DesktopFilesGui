using System.Collections.Generic;
using System.Linq;

namespace DesktopFilesGui.Extensions;

public static class DictionaryExtensions
{
    public static bool ContainsRangeKey<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IEnumerable<TKey> keys)
    {
        return keys
            .Select(dictionary.ContainsKey)
            .All(result => result);
    }
}