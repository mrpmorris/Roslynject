using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Runtime.CompilerServices;

namespace Morris.Roslynjector.Generator.IncrementalValueProviders.InjectionCandidates;

internal static class InjectionCandidatesFactory
{
    public static IncrementalValuesProvider<INamedTypeSymbol> CreateValuesProvider(
        IncrementalGeneratorInitializationContext context)
    =>
        context
        .SyntaxProvider
        .CreateSyntaxProvider(
            predicate: SyntaxNodePredicate,
            transform: static (context, _) =>
            {
                INamedTypeSymbol result = (INamedTypeSymbol)context.SemanticModel.GetDeclaredSymbol(context.Node)!;
                return result;
            }
        )
        .WithComparer(new ClassSignatureComparer());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool SyntaxNodePredicate(
        SyntaxNode syntaxNode,
        CancellationToken cancellationToken)
    =>
        syntaxNode is ClassDeclarationSyntax classNode
        && classNode.TypeParameterList is null
        && !classNode.Modifiers.Any(SyntaxKind.AbstractKeyword)
        && !classNode.Modifiers.Any(SyntaxKind.StaticKeyword);
}
