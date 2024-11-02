using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Morris.Roslynjector.Generator.Extensions;
using Morris.Roslynjector.Generator.IncrementalValueProviders.AttributeMetas;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
namespace Morris.Roslynjector.Generator.IncrementalValueProviders.RegistrationClassMetas;

internal static class RegistrationClassMetasFactory
{
    public static IncrementalValuesProvider<RegistrationClassMeta> CreateValuesProvider(
        IncrementalGeneratorInitializationContext context)
    =>
        context
        .SyntaxProvider
        .CreateSyntaxProvider(
            predicate: SyntaxNodePredicate,
            transform: TransformSyntaxContext
         )
        .Where(x => x is not null);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool SyntaxNodePredicate(SyntaxNode syntaxNode, CancellationToken cancellationToken)
    {
        if (syntaxNode is not ClassDeclarationSyntax classDeclarationSyntax)
            return false;

        return classDeclarationSyntax
            .AttributeLists
            .SelectMany(x => x.Attributes)
            .Any(attr =>
                AttributeNames.ShortNames.Any(
                    name => attr.Name.ToFullString().Contains(name)
                )
             );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static RegistrationClassMeta TransformSyntaxContext(
        GeneratorSyntaxContext context,
        CancellationToken cancellationToken)
    {
        var attributes =
            context
            .Node
            .DescendantNodes()
            .OfType<AttributeSyntax>()
            .Select(x =>
                RegisterAttributeMetaFactory.Create(
                    attributeSyntax: x,
                    semanticModel: context.SemanticModel,
                    cancellationToken: cancellationToken
                )!
            )
            .Where(x => x is not null)
            .ToImmutableArray();
        if (attributes.Length == 0)
            return null!;

        (string? Namespace, string Name)? namespaceAndName = context.GetNamespaceAndName(cancellationToken);
        if (namespaceAndName is null)
            return null!;

        return new RegistrationClassMeta(
            @namespace: namespaceAndName.Value.Namespace,
            className: namespaceAndName.Value.Name,
            attributes: attributes);
    }
}
