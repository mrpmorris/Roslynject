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
            "RegisterClassesOfTypeAttribute" =>
                CreateDeclaredRegisterClassesOfTypeAttribute(
                    semanticModel: semanticModel,
                    attributeSyntax: attributeSyntax,
                    serviceLifetime: serviceLifetime,
                    baseClassTypeArgument: arguments[1],
                    cancellationToken: cancellationToken),

            "RegisterClassesWhereNameEndsWithAttribute" =>
                CreateDeclaredRegisterClassesWhereNameEndsWithAttribute(
                    attributeSyntax: attributeSyntax,
                    serviceLifetime: serviceLifetime,
                    suffixArgument: arguments[1]),

            "RegisterInterfacesOfTypeAttribute" =>
                CreateDeclaredRegisterInterfacesOfTypeAttribute(
                    semanticModel: semanticModel,
                    attributeSyntax: attributeSyntax,
                    serviceLifetime: serviceLifetime,
                    baseInterfaceTypeArgument: arguments[1],
                    cancellationToken: cancellationToken),

            "RegisterInterfacesWhereNameEndsWithAttribute" =>
                CreateRegisterInterfacesWhereNameEndsWithAttribute(
                    attributeSyntax: attributeSyntax,
                    serviceLifetime: serviceLifetime,
                    suffixArgument: arguments[1]),

            _ => null
        };
    }

    private static DeclaredRegisterAttributeBase? CreateDeclaredRegisterClassesOfTypeAttribute(
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
            : new DeclaredRegisterClassesOfTypeAttribute(
                attributeSyntax: attributeSyntax,
                serviceLifetime: serviceLifetime,
                baseClassType: baseClassType);
    }

    private static DeclaredRegisterAttributeBase CreateDeclaredRegisterClassesWhereNameEndsWithAttribute(
        AttributeSyntax attributeSyntax,
        ServiceLifetime serviceLifetime,
        AttributeArgumentSyntax suffixArgument)
    {
        string suffix = suffixArgument.Expression.ToString().Trim('"');
        return new DeclaredRegisterClassesWhereNameEndsWithAttribute(
            attributeSyntax: attributeSyntax,
            serviceLifetime: serviceLifetime,
            suffix: suffix);
    }

    private static DeclaredRegisterAttributeBase? CreateDeclaredRegisterInterfacesOfTypeAttribute(
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
            : new DeclaredRegisterInterfacesOfTypeAttribute(
                attributeSyntax: attributeSyntax,
                serviceLifetime: serviceLifetime,
                baseInterfaceType: baseInterfaceType);
    }

    private static DeclaredRegisterAttributeBase CreateRegisterInterfacesWhereNameEndsWithAttribute(
        AttributeSyntax attributeSyntax,
        ServiceLifetime serviceLifetime,
        AttributeArgumentSyntax suffixArgument)
    {
        string suffix = suffixArgument.Expression.ToString().Trim('"');
        return new DeclaredRegisterInterfacesWhereNameEndsWithAttribute(
            attributeSyntax: attributeSyntax,
            serviceLifetime: serviceLifetime,
            suffix: suffix);
    }
}

