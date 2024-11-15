using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace Morris.Roslynject.Extensions;

internal static class GeneratorSyntaxContextExtensions
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static (string? Namespace, string Name)? GetNamespaceAndName(
		this GeneratorSyntaxContext context, CancellationToken cancellationToken)
	{
		var typeDeclarationSyntax = (TypeDeclarationSyntax)context.Node;
		var typeSymbol = (INamedTypeSymbol?)context.SemanticModel.GetDeclaredSymbol(typeDeclarationSyntax, cancellationToken);

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
