using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace Morris.Roslynject.Extensions;

internal static class AttributeSyntaxGetArgumentsExtension
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ImmutableDictionary<string, object?> GetArguments(
		this AttributeSyntax source,
		SemanticModel semanticModel,
		CancellationToken cancellationToken)
	{
		SeparatedSyntaxList<AttributeArgumentSyntax>? arguments = source.ArgumentList?.Arguments;
		if (arguments is null)
			return ImmutableDictionary<string, object?>.Empty;

		var builder = ImmutableDictionary.CreateBuilder<string, object?>(StringComparer.CurrentCultureIgnoreCase);

		for (int argumentIndex = 0; argumentIndex < arguments.Value.Count; argumentIndex++)
		{
			AttributeArgumentSyntax argument = arguments.Value[argumentIndex];

			// Retrieve the argument name or fall back to the constructor parameter name
			string argumentName =
				argument.NameEquals is not null
				? argument.NameEquals.Name.Identifier.ValueText
				//: argument.NameColon is not null
				//? argument.NameColon.Name.Identifier.ValueText
				//: parameters != null && argumentIndex < parameters.Length
				//? parameters[argumentIndex].Name
				: $"arg{argumentIndex}";

			ExpressionSyntax argumentExpression = argument.Expression;
			object? argumentValue = argumentExpression.GetValue(semanticModel, cancellationToken);
			builder[argumentName] = argumentValue;
		}

		return builder.ToImmutable();
	}
}