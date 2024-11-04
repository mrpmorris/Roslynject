using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace Morris.Roslynject.Generator.Extensions;

internal static class ImmutableArrayTGetContentsHashCodeExtension
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetContentsHashCode<T>(
        this ImmutableArray<T> source,
        Func<T, int>? getHashCode = null)
    {
        if (source.IsDefaultOrEmpty)
            return 0;

        getHashCode ??= DefaultGetHashCode;
        int result = 17;

        unchecked
        {
            for (int i = 0; i < source.Length; i++)
                result = result * 31 + (getHashCode(source[i]));
        }
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int DefaultGetHashCode<T>(T? source) =>
        source is null ? 0 : source.GetHashCode();
}
