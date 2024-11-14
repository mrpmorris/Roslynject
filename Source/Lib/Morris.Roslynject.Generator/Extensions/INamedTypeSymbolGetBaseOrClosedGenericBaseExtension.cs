using Microsoft.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Morris.Roslynject.Generator.Extensions;

internal static partial class INamedTypeSymbolExtensions
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static INamedTypeSymbol? GetBaseOrClosedGenericType(this INamedTypeSymbol child, INamedTypeSymbol baseClass)
	{
		if (!baseClass.IsGenericType)
			return baseClass;

		INamedTypeSymbol? result = null;

		INamedTypeSymbol? current = child.BaseType;
		while (current is not null)
		{
			if (TypeIdentityComparer.Default.Equals(current.ConstructedFrom, baseClass.ConstructedFrom))
				return baseClass;
			if (
				!current.IsGenericType
				&& !current
					.TypeArguments
					.Any(x => x.Kind == SymbolKind.TypeParameter)
			)
			{
				result = current;
			}
			current = current.BaseType;
		}
		return result;
	}
}
