using Microsoft.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Morris.Roslynject.Extensions;

internal static class INamedTypeSymbolIsOfTypeExtension
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsOfType(this INamedTypeSymbol child, INamedTypeSymbol baseClass)
	=>
		TypeIdentityComparer.Default.Equals(child.ConstructedFrom, baseClass.ConstructedFrom)
		|| child.DescendsFrom(baseClass);

}
