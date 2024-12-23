﻿using Microsoft.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Morris.Roslynject.Extensions;

internal static class INamedTypeSymbolGetBaseClosedGenericExtension
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static INamedTypeSymbol? GetBaseClosedGenericType(this INamedTypeSymbol descendantType, INamedTypeSymbol baseClass)
	{
		if (!baseClass.IsGenericType)
			return baseClass;

		INamedTypeSymbol? currentType = descendantType.BaseType;
		while (currentType != null)
		{
			if (SymbolEqualityComparer.Default.Equals(
				currentType.ConstructedFrom,
				baseClass.ConstructedFrom))
			{
				return currentType;
			}

			currentType = currentType.BaseType;
		}
		return null;
	}
}
