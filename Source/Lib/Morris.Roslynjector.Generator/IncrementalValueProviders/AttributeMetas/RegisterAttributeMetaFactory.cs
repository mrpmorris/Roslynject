using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using Morris.Roslynjector.Generator.IncrementalValueProviders.AttributeMetas;
using System.Collections.Immutable;

namespace Morris.Roslynjector.Generator.IncrementalValueProviders.AttributeMetas;

internal static class RegisterAttributeMetaFactory
{
    public static RegisterAttributeMetaBase? Create(
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
        switch (attributeName)
        {
            case "RegisterInterfacesAttribute":
                var interfaceType = semanticModel.GetTypeInfo(arguments[1].Expression, cancellationToken).Type as INamedTypeSymbol;
                return interfaceType is null
                    ? null
                    : new RegisterInterfacesAttributeMeta(
                        serviceLifetime,
                        interfaceType,
                        ImmutableArray<INamedTypeSymbol>.Empty);

            case "RegisterInterfacesWhereDescendsFromAttribute":
                var baseInterface = semanticModel.GetTypeInfo(arguments[1].Expression, cancellationToken).Type as INamedTypeSymbol;
                return baseInterface is null
                    ? null
                    : new RegisterInterfacesWhereDescendsFromAttributeMeta(
                        serviceLifetime,
                        baseInterface,
                        ImmutableArray<INamedTypeSymbol>.Empty);

            case "RegisterInterfacesWhereNameEndsWithAttribute":
                var suffix = arguments[1].Expression.ToString().Trim('"');
                return new RegisterInterfacesWhereNameEndsWithAttributeMeta(
                    serviceLifetime,
                    suffix,
                    ImmutableArray<INamedTypeSymbol>.Empty);

            case "RegisterClassesWhereDescendsFromAttribute":
                var baseClass = semanticModel
                    .GetTypeInfo(
                        ((TypeOfExpressionSyntax)arguments[1].Expression).Type,
                        cancellationToken)
                    .Type as INamedTypeSymbol;
                return baseClass is null
                    ? null
                    : new RegisterClassesWhereDescendsFromAttributeMeta(
                        serviceLifetime,
                        baseClass,
                        ImmutableArray<INamedTypeSymbol>.Empty);

            case "RegisterClassesWhereNameEndsWithAttribute":
                suffix = arguments[1].Expression.ToString().Trim('"');
                return new RegisterClassesWhereNameEndsWithAttributeMeta(
                    serviceLifetime,
                    suffix,
                    ImmutableArray<INamedTypeSymbol>.Empty);

            default:
                return null;
        }
    }
}
