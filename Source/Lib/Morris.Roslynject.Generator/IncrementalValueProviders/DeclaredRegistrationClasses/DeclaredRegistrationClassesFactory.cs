using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Morris.Roslynject.Generator.Extensions;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
namespace Morris.Roslynject.Generator.IncrementalValueProviders.DeclaredRegistrationClasses;

internal static class DeclaredRegistrationClassesFactory
{
    public static IncrementalValuesProvider<DeclaredRegistrationClass> CreateValuesProvider(
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
    private static bool SyntaxNodePredicate(
        SyntaxNode syntaxNode,
        CancellationToken cancellationToken)
    =>
        syntaxNode is ClassDeclarationSyntax classDeclarationSyntax
        && classDeclarationSyntax.IsConcrete();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static DeclaredRegistrationClass TransformSyntaxContext(
        GeneratorSyntaxContext context,
        CancellationToken cancellationToken)
    {
        INamedTypeSymbol classSymbol =
            (INamedTypeSymbol)context
            .SemanticModel
            .GetDeclaredSymbol(context.Node)!;

        INamedTypeSymbol? roslynjectModuleType = context
            .SemanticModel
            .Compilation
            .GetTypesByMetadataName("Morris.Roslynject.RoslynjectModule")
            .First();

        if (!classSymbol.DescendsFrom(roslynjectModuleType))
            return null!;
        
        var attributes =
            context
            .Node
            .DescendantNodes()
            .DescendedFrom<AttributeSyntax>()
            .Select(x =>
                DeclaredRegisterAttributeFactory.Create(
                    attributeSyntax: x,
                    semanticModel: context.SemanticModel,
                    cancellationToken: cancellationToken
                )!
            )
            .Where(x => x is not null)
            .ToImmutableArray();

        (string? Namespace, string Name)? namespaceAndName = context.GetNamespaceAndName(cancellationToken);
        if (namespaceAndName is null)
            return null!;

        return new DeclaredRegistrationClass(
            namespaceName: namespaceAndName.Value.Namespace,
            className: namespaceAndName.Value.Name,
            attributes: attributes);
    }
}
