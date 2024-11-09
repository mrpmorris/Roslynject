using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using Morris.Roslynject.Generator.Extensions;
using System.Collections.Immutable;

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

		ImmutableDictionary<string, object?> arguments =
			attributeSyntax.GetArguments(semanticModel, cancellationToken);

		var baseClass = arguments.GetValue<INamedTypeSymbol>("BaseClass");
		var serviceLifetime = arguments.GetValue<ServiceLifetime>("ServiceLifetime");
		var registerAs = arguments.GetValue<ClassRegistration>("RegisterAs");
		var classRegex = arguments.GetValue<string?>("ClassRegex");

		// Determine the specific attribute type and create the corresponding meta object
		string attributeName = attributeTypeSymbol.Name;
		return attributeName switch {
			"RegisterClassesDescendedFromAttribute" =>
				CreateDeclaredRegisterClassesDescendedFromAttribute(
					attributeSyntax: attributeSyntax,
					baseClass: baseClass,
					serviceLifetime: serviceLifetime,
					registerAs: registerAs,
					classRegex: classRegex,
					cancellationToken: cancellationToken),

			_ => null
		};
	}

	private static DeclaredRegisterAttributeBase? CreateDeclaredRegisterClassesDescendedFromAttribute(
		AttributeSyntax attributeSyntax,
		INamedTypeSymbol baseClass,
		ServiceLifetime serviceLifetime,
		ClassRegistration registerAs,
		string? classRegex,
		CancellationToken cancellationToken)
	=>
		new DeclaredRegisterClassesDescendedFromAttribute(
			attributeSyntax: attributeSyntax,
			baseClassType: baseClass,
			serviceLifetime: serviceLifetime,
			registerAs: registerAs,
			classRegex: classRegex);
}

