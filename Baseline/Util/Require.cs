using System.Diagnostics.CodeAnalysis;
using ITI.DDD.Core;

namespace ITI.Baseline.Util;

public static class Require
{
    public static void NotNull([NotNull] object? obj, string message)
    {
        if (obj == null)
            throw new ValidationException(message);
    }

    public static void HasValue([NotNull] string? s, string message)
    {
        if (string.IsNullOrEmpty(s?.Trim()))
            throw new ValidationException(message);
    }

    public static void IsTrue(bool b, string message)
    {
        if (!b) throw new ValidationException(message);
    }
}
