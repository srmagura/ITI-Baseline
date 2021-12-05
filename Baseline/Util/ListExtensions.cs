using System.Diagnostics.CodeAnalysis;

namespace ITI.Baseline.Util;

public static class ListExtensions
{
    public static bool HasItems<T>([NotNullWhen(true)] this List<T>? list)
    {
        return list != null && list.Count > 0;
    }
}
