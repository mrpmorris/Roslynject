using Microsoft.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Morris.Roslynjector.Generator.Helpers;

internal static class NamespaceHelper
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Combine(string? namespaceName, string className) =>
        namespaceName is null
        ? className
        : namespaceName + "." + className;
}