using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Morris.Roslynject.StaticResources;
using System.Threading;

namespace Morris.Roslynject.Extensions;

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
				GetValueFromMemberAccessExpression(memberAccessExpression, semanticModel, cancellationToken),


			_ => throw new NotImplementedException()
		};

	private static object? GetValueFromMemberAccessExpression(
		MemberAccessExpressionSyntax expression,
		SemanticModel semanticModel,
		CancellationToken cancellationToken)
	{
		ISymbol? symbol = semanticModel
			.GetSymbolInfo(expression, cancellationToken)
			.Symbol;

		if (symbol is null)
			return ParseGeneratedEnumValue(expression);

		if (
			symbol is IFieldSymbol field
			&& field.ContainingType.TypeKind == TypeKind.Enum)
		{
			return field.ConstantValue;
		}

		return null;
	}

	private static object? ParseGeneratedEnumValue(MemberAccessExpressionSyntax expression)
	{
		string[] values = expression.ToFullString().Split('.');

		if (values.Length != 2)
			return null;

		return values[0] switch {
			nameof(SourceCode.RegisterClassAs) => RegisterClassAsParser.Parse(values[1]),
			nameof(SourceCode.RegisterInterfaceAs) => RegisterInterfaceAsParser.Parse(values[1]),
			_ => null
		};
	}
}
