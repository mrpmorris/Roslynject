using Microsoft.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Morris.Roslynject.Extensions;

internal static class INamedTypeSymbolGetNamespaceAndNameExtension
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static (string? NamespaceName, string Name) GetNamespaceNameAndClassName(
		this INamedTypeSymbol symbol, CancellationToken cancellationToken)
	=>
		(
			NamespaceName: symbol.ContainingNamespace.IsGlobalNamespace == false
				? symbol.ContainingNamespace.ToDisplayString()
				: null,
			symbol.Name
		);

}
