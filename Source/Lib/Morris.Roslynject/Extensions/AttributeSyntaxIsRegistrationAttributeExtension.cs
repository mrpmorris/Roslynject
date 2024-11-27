using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Runtime.CompilerServices;

namespace Morris.Roslynject.Extensions;

internal static class AttributeSyntaxIsRegistrationAttributeExtension
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsRegistrationAttribute(this AttributeSyntax attribute, SemanticModel semanticModel)
	{
		INamedTypeSymbol containingType = semanticModel.GetSymbolInfo(attribute).Symbol!.ContainingType!;
		string attributeFullName = containingType.ToDisplayString();

		return AttributeNames
			.FullNames
			.Contains(attributeFullName);
	}
}
