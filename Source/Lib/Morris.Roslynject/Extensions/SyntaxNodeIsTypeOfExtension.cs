using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;

namespace Morris.Roslynject.Extensions;

internal static class SyntaxNodeIsTypeOfExtension
{
	public static bool IsTypeOf(
		this SyntaxNode node,
		SemanticModel semanticModel,
		string attributeFullName)
	=>
		semanticModel.GetSymbolInfo(node).Symbol is IMethodSymbol methodSymbol
		&& methodSymbol.ContainingType.ToDisplayString() == attributeFullName;
}
