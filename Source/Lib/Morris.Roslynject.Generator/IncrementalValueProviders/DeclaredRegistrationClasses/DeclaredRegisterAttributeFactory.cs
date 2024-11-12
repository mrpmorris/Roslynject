using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using Morris.Roslynject.Generator.Extensions;
using System.Collections.Immutable;
using Morris.Roslynject.Generator.Morris.Roslynject;

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
		var registerClassAs = arguments.GetValue<RegisterClassAs>("RegisterClassAs");
		var classRegex = arguments.GetValueOrDefault<string?>("ClassRegex");

		// Determine the specific attribute type and create the corresponding meta object
		string attributeName = attributeTypeSymbol.Name;
		return attributeName switch {
			"RegisterClassesDescendedFromAttribute" =>
				CreateDeclaredRegisterClassesDescendedFromAttribute(
					attributeSyntax: attributeSyntax,
					baseClass: baseClass,
					serviceLifetime: serviceLifetime,
					registerAs: registerClassAs,
					classRegex: classRegex,
					cancellationToken: cancellationToken),

			_ => null
		};
	}

	private static DeclaredRegisterAttributeBase? CreateDeclaredRegisterClassesDescendedFromAttribute(
		AttributeSyntax attributeSyntax,
		INamedTypeSymbol baseClass,
		ServiceLifetime serviceLifetime,
		RegisterClassAs registerAs,
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

