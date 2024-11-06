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
        return attributeName switch
        {
            "RegisterClassesDescendedFromAttribute" =>
                CreateDeclaredRegisterClassesDescendedFromAttribute(
                    semanticModel: semanticModel,
                    attributeSyntax: attributeSyntax,
                    serviceLifetime: serviceLifetime,
                    baseClassTypeArgument: arguments[1],
                    cancellationToken: cancellationToken),

            "RegisterInterfacesDescendedFromAttribute" =>
                CreateDeclaredRegisterInterfacesDescendedFromAttribute(
                    semanticModel: semanticModel,
                    attributeSyntax: attributeSyntax,
                    serviceLifetime: serviceLifetime,
                    baseInterfaceTypeArgument: arguments[1],
                    cancellationToken: cancellationToken),

            _ => null
        };
    }

    private static DeclaredRegisterAttributeBase? CreateDeclaredRegisterClassesDescendedFromAttribute(
        SemanticModel semanticModel,
        AttributeSyntax attributeSyntax,
        ServiceLifetime serviceLifetime,
        AttributeArgumentSyntax baseClassTypeArgument,
        CancellationToken cancellationToken)
    {
        INamedTypeSymbol? baseClassType = baseClassTypeArgument
            .GetNamedTypeSymbol(semanticModel, cancellationToken);

        return baseClassType is null
            ? null
            : new DeclaredRegisterClassesDescendedFromAttribute(
                attributeSyntax: attributeSyntax,
                serviceLifetime: serviceLifetime,
                baseClassType: baseClassType);
    }

    private static DeclaredRegisterAttributeBase? CreateDeclaredRegisterInterfacesDescendedFromAttribute(
        SemanticModel semanticModel,
        AttributeSyntax attributeSyntax,
        ServiceLifetime serviceLifetime,
        AttributeArgumentSyntax baseInterfaceTypeArgument,
        CancellationToken cancellationToken)
    {
        INamedTypeSymbol? baseInterfaceType = baseInterfaceTypeArgument
            .GetNamedTypeSymbol(semanticModel, cancellationToken);

        return baseInterfaceType is null
            ? null
            : new DeclaredRegisterInterfacesDescendedFromAttribute(
                attributeSyntax: attributeSyntax,
                serviceLifetime: serviceLifetime,
                baseInterfaceType: baseInterfaceType);
    }
}

