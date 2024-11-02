using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;

namespace Morris.Roslynjector.Generator.Extensions;

internal static class AttributeSyntaxExtensions
{
    public static ImmutableDictionary<string, object?> GetArguments(
        this AttributeSyntax source,
        GeneratorSyntaxContext context)
    {
        SeparatedSyntaxList<AttributeArgumentSyntax>? arguments = source.ArgumentList?.Arguments;
        if (arguments is null)
            return ImmutableDictionary<string, object?>.Empty;

        SemanticModel semanticModel = context.SemanticModel;

        var builder = ImmutableDictionary.CreateBuilder<string, object?>();

        // Get the symbol for the attribute type
        var attributeSymbol = semanticModel.GetSymbolInfo(source).Symbol?.ContainingType;

        // Retrieve constructor parameters if available
        IMethodSymbol constructor = attributeSymbol?.Constructors.First();
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
            Optional<object> argumentValue = semanticModel.GetConstantValue(argumentExpression);
            builder[argumentName] = argumentValue.HasValue ? argumentValue.Value : null;
        }

        return builder.ToImmutable();
    }
}