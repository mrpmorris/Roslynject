using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Runtime.CompilerServices;
using Morris.Roslynject.Extensions;

namespace Morris.Roslynject.IncrementalValueProviders.InjectionCandidates;

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
				var result = (INamedTypeSymbol)context
					.SemanticModel
					.GetDeclaredSymbol(context.Node)!;
				return result;
			}
		);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static bool SyntaxNodePredicate(
		SyntaxNode syntaxNode,
		CancellationToken cancellationToken)
	=>
		syntaxNode is ClassDeclarationSyntax classNode
		&& classNode.TypeParameterList is null
		&& classNode.IsConcrete();
}
