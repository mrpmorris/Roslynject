using Microsoft.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Morris.Roslynject.Extensions;

internal static class INamedTypeSymbolGetNamespaceAndNameExtension
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static (string? NamespaceName, string Name) GetNamespaceAndName(
		this INamedTypeSymbol symbol, CancellationToken cancellationToken)
	=>
		(
			NamespaceName: symbol.ContainingNamespace.IsGlobalNamespace == false
				? symbol.ContainingNamespace.ToDisplayString()
				: null,
			symbol.Name
		);

}
