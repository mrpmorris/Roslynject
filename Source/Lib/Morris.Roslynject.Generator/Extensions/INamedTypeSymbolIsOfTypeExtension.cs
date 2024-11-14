using Microsoft.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Morris.Roslynject.Generator.Extensions;

internal static partial class INamedTypeSymbolExtensions
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsOfType(this INamedTypeSymbol child, INamedTypeSymbol baseClass)
	=>
		TypeIdentityComparer.Default.Equals(child.ConstructedFrom, baseClass.ConstructedFrom)
		|| child.DescendsFrom(baseClass);

}
