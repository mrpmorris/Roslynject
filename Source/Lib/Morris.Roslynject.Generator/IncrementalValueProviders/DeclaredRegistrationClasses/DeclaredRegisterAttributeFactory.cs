using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using Morris.Roslynject.Generator.Extensions;

namespace Morris.Roslynject.Generator.IncrementalValueProviders.DeclaredRegistrationClasses;

internal static class DeclaredRegisterAttributeFactory
{
	public static DeclaredRegisterAttributeBase? Create(
		AttributeSyntax attributeSyntax,
		SemanticModel semanticModel,
		CancellationToken cancellationToken)
	{
		if (semanticModel.GetTypeInfo(attributeSyntax, cancellationToken).Type is not INamedTypeSymbol attributeTypeSymbol)
			return null;

		if (attributeSyntax.ArgumentList?.Arguments is not { Count: > 0 } arguments)
			return null;

		Optional<object?> lifetimeArgument =
			semanticModel.GetConstantValue(arguments[0].Expression, cancellationToken);
		if (!lifetimeArgument.HasValue || lifetimeArgument.Value is not int lifetimeValue)
			return null;

		var serviceLifetime = (ServiceLifetime)lifetimeValue;
		// Determine the specific attribute type and create the corresponding meta object
		string attributeName = attributeTypeSymbol.Name;
		return attributeName switch {
			"RegisterClassesDescendedFromAttribute" =>
				CreateDeclaredRegisterClassesDescendedFromAttribute(
					semanticModel: semanticModel,
					attributeSyntax: attributeSyntax,
					baseClassTypeArgument: arguments[1],
					serviceLifetime: serviceLifetime,
					registerAs: ClassRegistration.BaseClass, // TODO: PeteM - Don't hard-code
					classRegex: null, // TODO: PeteM - Don't hard-code
					cancellationToken: cancellationToken),

			_ => null
		};
	}

	private static DeclaredRegisterAttributeBase? CreateDeclaredRegisterClassesDescendedFromAttribute(
		SemanticModel semanticModel,
		AttributeSyntax attributeSyntax,
		AttributeArgumentSyntax baseClassTypeArgument,
		ServiceLifetime serviceLifetime,
		ClassRegistration registerAs,
		string? classRegex,
		CancellationToken cancellationToken)
	{
		INamedTypeSymbol? baseClassType = baseClassTypeArgument
			.GetNamedTypeSymbol(semanticModel, cancellationToken);

		return baseClassType is null
			? null
			: new DeclaredRegisterClassesDescendedFromAttribute(
				attributeSyntax: attributeSyntax,
				baseClassType: baseClassType,
				serviceLifetime: serviceLifetime,
				registerAs: registerAs,
				classRegex: classRegex);
	}
}

