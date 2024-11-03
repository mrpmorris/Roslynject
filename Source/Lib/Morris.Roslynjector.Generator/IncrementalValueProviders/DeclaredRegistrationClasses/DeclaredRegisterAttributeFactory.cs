using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using Morris.Roslynjector.Generator.Extensions;

namespace Morris.Roslynjector.Generator.IncrementalValueProviders.DeclaredRegistrationClasses;

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
            "RegisterClassesWhereDescendsFromAttribute" =>
                CreateDeclaredRegisterClassesWhereDescendsFromAttribute(
                    semanticModel: semanticModel,
                    serviceLifetime: serviceLifetime,
                    baseClassTypeArgument: arguments[1],
                    cancellationToken: cancellationToken),

            "RegisterClassesWhereNameEndsWithAttribute" =>
                CreateDeclaredRegisterClassesWhereNameEndsWithAttribute(
                    serviceLifetime: serviceLifetime,
                    suffixArgument: arguments[1]),

            "RegisterInterfaceAttribute" =>
                CreateDeclaredRegisterInterfaceAttribute(
                    semanticModel: semanticModel,
                    serviceLifetime: serviceLifetime,
                    interfaceTypeArgument: arguments[1],
                    cancellationToken: cancellationToken),

            "RegisterInterfacesWhereDescendsFromAttribute" =>
                CreateDeclaredRegisterInterfacesWhereDescendsFromAttribute(
                    semanticModel: semanticModel,
                    serviceLifetime: serviceLifetime,
                    baseInterfaceTypeArgument: arguments[1],
                    cancellationToken: cancellationToken),

            "RegisterInterfacesWhereNameEndsWithAttribute" =>
                CreateRegisterInterfacesWhereNameEndsWithAttribute(
                    serviceLifetime: serviceLifetime,
                    suffixArgument: arguments[1]),

            _ => null
        };
    }

    private static DeclaredRegisterAttributeBase? CreateDeclaredRegisterClassesWhereDescendsFromAttribute(
        SemanticModel semanticModel,
        ServiceLifetime serviceLifetime,
        AttributeArgumentSyntax baseClassTypeArgument,
        CancellationToken cancellationToken)
    {
        INamedTypeSymbol? baseClassType = baseClassTypeArgument
            .GetNamedTypeSymbol(semanticModel, cancellationToken);

        return baseClassType is null
            ? null
            : new DeclaredRegisterClassesWhereDescendsFromAttribute(
                serviceLifetime: serviceLifetime,
                baseClassType: baseClassType);
    }

    private static DeclaredRegisterAttributeBase CreateDeclaredRegisterClassesWhereNameEndsWithAttribute(
        ServiceLifetime serviceLifetime,
        AttributeArgumentSyntax suffixArgument)
    {
        string suffix = suffixArgument.Expression.ToString().Trim('"');
        return new DeclaredRegisterClassesWhereNameEndsWithAttribute(
            serviceLifetime: serviceLifetime,
            suffix: suffix);
    }

    private static DeclaredRegisterAttributeBase? CreateDeclaredRegisterInterfaceAttribute(
        SemanticModel semanticModel,
        ServiceLifetime serviceLifetime,
        AttributeArgumentSyntax interfaceTypeArgument,
        CancellationToken cancellationToken)
    {
        INamedTypeSymbol? interfaceType = interfaceTypeArgument
            .GetNamedTypeSymbol(semanticModel, cancellationToken);

        return interfaceType is null
            ? null
            : new DeclaredRegisterInterfaceAttribute(
                serviceLifetime: serviceLifetime,
                interfaceType: interfaceType);
    }

    private static DeclaredRegisterAttributeBase? CreateDeclaredRegisterInterfacesWhereDescendsFromAttribute(
        SemanticModel semanticModel,
        ServiceLifetime serviceLifetime,
        AttributeArgumentSyntax baseInterfaceTypeArgument,
        CancellationToken cancellationToken)
    {
        INamedTypeSymbol? baseInterfaceType = baseInterfaceTypeArgument
            .GetNamedTypeSymbol(semanticModel, cancellationToken);

        return baseInterfaceType is null
            ? null
            : new DeclaredRegisterInterfacesWhereDescendsFromAttribute(
                serviceLifetime: serviceLifetime,
                baseInterfaceType: baseInterfaceType);
    }

    private static DeclaredRegisterAttributeBase CreateRegisterInterfacesWhereNameEndsWithAttribute(
        ServiceLifetime serviceLifetime,
        AttributeArgumentSyntax suffixArgument)
    {
        string suffix = suffixArgument.Expression.ToString().Trim('"');
        return new DeclaredRegisterInterfacesWhereNameEndsWithAttribute(
            serviceLifetime: serviceLifetime,
            suffix: suffix);
    }
}

