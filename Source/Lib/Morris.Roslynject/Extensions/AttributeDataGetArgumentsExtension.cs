using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace Morris.Roslynject.Extensions;

internal static class AttributeDataGetArgumentsExtension
{
	public static ImmutableDictionary<string, object?> GetArguments(this AttributeData attributeData)
	{
		var builder = ImmutableDictionary.CreateBuilder<string, object?>(StringComparer.CurrentCultureIgnoreCase);

		// Handle positional constructor arguments
		if (!attributeData.ConstructorArguments.IsDefaultOrEmpty)
		{
			IEnumerable<IParameterSymbol> parameters = attributeData.AttributeConstructor?.Parameters ?? Enumerable.Empty<IParameterSymbol>();
			for (int argumentIndex = 0; argumentIndex < attributeData.ConstructorArguments.Length; argumentIndex++)
			{
				// Attempt to get the parameter name from constructor if available
				string argumentName =
					argumentIndex < parameters.Count()
						? parameters.ElementAt(argumentIndex).Name
						: $"arg{argumentIndex}";

				// Convert the argument value to a native object representation
				object? argumentValue = attributeData.ConstructorArguments[argumentIndex].GetValue();
				builder[argumentName] = argumentValue;
			}
		}

		// Handle named arguments
		if (!attributeData.NamedArguments.IsDefaultOrEmpty)
			foreach (var namedArgument in attributeData.NamedArguments)
				builder[namedArgument.Key] = namedArgument.Value.GetValue();

		// Return the arguments as an immutable dictionary
		return builder.ToImmutable();
	}

	public static object? GetValue(this TypedConstant typedConstant)
	{
		if (typedConstant.IsNull)
			return null;

		if (typedConstant.Kind == TypedConstantKind.Array)
			return typedConstant.Values.Select(v => v.GetValue()).ToArray();

		return typedConstant.Value;
	}
}