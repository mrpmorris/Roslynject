using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Morris.Roslynject.Extensions;

internal static class AttributeArgumentGetNamedTypeSymbolExtension
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static INamedTypeSymbol? GetNamedTypeSymbol(
		this AttributeArgumentSyntax syntax,
		SemanticModel semanticModel,
		CancellationToken cancellationToken)
	=>
		 semanticModel
			.GetTypeInfo(
				((TypeOfExpressionSyntax)syntax.Expression).Type,
				cancellationToken)
			.Type as INamedTypeSymbol;
}
