using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using Morris.Roslynject.Extensions;
using Morris.Roslynject.Generator.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Morris.Roslynject.IncrementalValueProviders.DeclaredRegistrationClasses;

internal static class DeclaredRegisterAttributeFactory
{
	public static DeclaredRegisterAttributeBase? Create(
		AttributeSyntax attributeSyntax,
		SemanticModel semanticModel,
		CancellationToken cancellationToken)
	{
		if (semanticModel.GetTypeInfo(attributeSyntax, cancellationToken).Type is not INamedTypeSymbol attributeTypeSymbol)
			return null;

		if (!attributeSyntax.IsRegistrationAttribute(semanticModel))
			return null;

		// Determine the specific attribute type and create the corresponding meta object
		string attributeName = attributeTypeSymbol.Name;
		if (!attributeName.EndsWith("Attribute"))
			attributeName = attributeName + "Attribute";

		return attributeName switch {
			nameof(RegisterClassesDescendedFromAttribute) =>
				CreateDeclaredRegisterClassesDescendedFromAttribute(
					attributeSyntax: attributeSyntax,
					semanticModel: semanticModel,
					cancellationToken: cancellationToken),

			_ => null
		};
	}

	private static DeclaredRegisterAttributeBase? CreateDeclaredRegisterClassesDescendedFromAttribute(
		AttributeSyntax attributeSyntax,
		SemanticModel semanticModel,
		CancellationToken cancellationToken)
	{
		ImmutableDictionary<string, object?> arguments =
			attributeSyntax.GetArguments(
				semanticModel,
				cancellationToken);

		var baseClass = arguments.GetValue<INamedTypeSymbol>("BaseClass");
		var serviceLifetime = arguments.GetValue<ServiceLifetime>("ServiceLifetime");
		var registerClassAs = arguments.GetValue<RegisterClassAs>("RegisterClassAs");
		var classRegex = arguments.GetValueOrDefault<string?>("ClassRegex");

		return new DeclaredRegisterClassesDescendedFromAttribute(
			attributeSyntax: attributeSyntax,
			baseClassType: baseClass,
			serviceLifetime: serviceLifetime,
			registerClassAs: registerClassAs,
			classRegex: classRegex);
	}
}

