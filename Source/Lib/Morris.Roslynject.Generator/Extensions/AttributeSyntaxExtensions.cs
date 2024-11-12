using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;

namespace Morris.Roslynject.Generator.Extensions;

internal static class AttributeSyntaxExtensions
{
	public static ImmutableDictionary<string, object?> GetArguments(
		this AttributeSyntax source,
		SemanticModel semanticModel,
		CancellationToken cancellationToken)
	{
		SeparatedSyntaxList<AttributeArgumentSyntax>? arguments = source.ArgumentList?.Arguments;
		if (arguments is null)
			return ImmutableDictionary<string, object?>.Empty;

		var builder = ImmutableDictionary.CreateBuilder<string, object?>(StringComparer.CurrentCultureIgnoreCase);

		// Get the symbol for the attribute type
		SymbolInfo attributeSymbolInfo = semanticModel.GetSymbolInfo(source);
		var attributeSymbol = attributeSymbolInfo.Symbol?.ContainingType;

		// Retrieve constructor parameters if available
		IMethodSymbol constructor = attributeSymbol!.Constructors.First();
		ImmutableArray<IParameterSymbol> parameters = constructor.Parameters;

		for (int argumentIndex = 0; argumentIndex < arguments.Value.Count; argumentIndex++)
		{
			AttributeArgumentSyntax argument = arguments.Value[argumentIndex];

			// Retrieve the argument name or fall back to the constructor parameter name
			string argumentName =
				argument.NameEquals is not null
				? argument.NameEquals.Name.Identifier.ValueText
				: argument.NameColon is not null
				? argument.NameColon.Name.Identifier.ValueText
				: parameters != null && argumentIndex < parameters.Length
				? parameters[argumentIndex].Name
				: $"arg{argumentIndex}";

			ExpressionSyntax argumentExpression = argument.Expression;
			object? argumentValue = argumentExpression.GetValue(semanticModel, cancellationToken);
			builder[argumentName] = argumentValue;
		}

		return builder.ToImmutable();
	}
}