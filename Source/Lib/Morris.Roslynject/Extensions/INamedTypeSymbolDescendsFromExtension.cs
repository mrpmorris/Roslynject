using Microsoft.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Morris.Roslynject.Extensions;

internal static partial class INamedTypeSymbolDescendsFromExtension
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool DescendsFrom(this INamedTypeSymbol child, INamedTypeSymbol baseClass)
	{
		INamedTypeSymbol? current = child.BaseType;
		while (current is not null)
		{
			if (TypeHierarchyComparer.Default.Equals(current.ConstructedFrom, baseClass.ConstructedFrom))
				return true;
			current = current.BaseType;
		}
		return false;
	}
}
