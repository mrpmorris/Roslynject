using Microsoft.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Morris.Roslynject.Extensions;

internal static class SyntaxNodeIsTypeOfExtension
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsTypeOf(
		this SyntaxNode node,
		SemanticModel semanticModel,
		string attributeFullName)
	=>
		semanticModel.GetSymbolInfo(node).Symbol is IMethodSymbol methodSymbol
		&& methodSymbol.ContainingType.ToDisplayString() == attributeFullName;
}
