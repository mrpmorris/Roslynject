using Microsoft.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Morris.Roslynject.Extensions;

internal static class INamedTypeSymbolIsTypeOfExtension
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsTypeOf(this INamedTypeSymbol child, INamedTypeSymbol baseClass)
	=>
		TypeHierarchyComparer.Default.Equals(child.ConstructedFrom, baseClass.ConstructedFrom)
		|| child.DescendsFrom(baseClass);

}
