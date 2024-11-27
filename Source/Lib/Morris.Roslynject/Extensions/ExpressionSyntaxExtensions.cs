using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Morris.Roslynject.Generator.Extensions;

internal static class ExpressionSyntaxExtensions
{
	public static object? GetValue(
		this ExpressionSyntax source,
		SemanticModel semanticModel,
		CancellationToken cancellationToken)
	=>
		source switch {
			TypeOfExpressionSyntax typeofExpression =>
				(INamedTypeSymbol)semanticModel.GetTypeInfo(typeofExpression.Type, cancellationToken).Type!,

			LiteralExpressionSyntax literalExpression =>
				literalExpression.Token.Value,

			MemberAccessExpressionSyntax memberAccessExpression =>
				semanticModel.GetSymbolInfo(memberAccessExpression, cancellationToken).Symbol switch {
					IFieldSymbol field when field.ContainingType.TypeKind == TypeKind.Enum =>
						field.ConstantValue,

					_ => throw new NotImplementedException()
				},

			_ => throw new NotImplementedException()
		};
}
