using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace Morris.Roslynject.Extensions;

internal static class GeneratorSyntaxContextExtensions
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static (string? Namespace, string Name)? GetNamespaceAndName(
		this GeneratorAttributeSyntaxContext context, CancellationToken cancellationToken)
	{
		ISymbol? typeSymbol = context.TargetSymbol;

		if (typeSymbol is null)
			return null;

		return
			(
				Namespace: typeSymbol.ContainingNamespace.IsGlobalNamespace == false
					? typeSymbol.ContainingNamespace.ToDisplayString()
					: null,
				typeSymbol.Name
			);
	}

}
