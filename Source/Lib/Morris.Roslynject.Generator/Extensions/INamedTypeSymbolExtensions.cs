using Microsoft.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Morris.Roslynject.Generator.Extensions;

internal static class INamedTypeSymbolExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool DescendsFrom(this INamedTypeSymbol child, INamedTypeSymbol baseClass)
    {
        INamedTypeSymbol? current = child.BaseType;
        while (current is not null)
        {
            if (TypeIdentityComparer.Default.Equals(current.ConstructedFrom, baseClass.ConstructedFrom))
                return true;
            current = current.BaseType;
        }
        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsOfType(this INamedTypeSymbol child, INamedTypeSymbol baseClass)
    =>
        TypeIdentityComparer.Default.Equals(child.ConstructedFrom, baseClass.ConstructedFrom)
        || child.DescendsFrom(baseClass);

}
