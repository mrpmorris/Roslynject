using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using System.Runtime.CompilerServices;
using Morris.Roslynject.Extensions;

namespace Morris.Roslynject.IncrementalValueProviders.DeclaredRoslynjectModules;

internal class DeclaredRoslynjectModuleIncrementalValuesProviderFactory
{
	public static IncrementalValuesProvider<DeclaredRoslynjectModule> CreateValuesProvider(
		IncrementalGeneratorInitializationContext context)
	=>
		context
		.SyntaxProvider
		.ForAttributeWithMetadataName(
			typeof(RoslynjectAttribute).FullName,
			predicate: SyntaxNodePredicate,
			transform: SyntaxNodeTransformer
		);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static bool SyntaxNodePredicate(
		SyntaxNode syntaxNode,
		CancellationToken cancellationToken)
	=>
		syntaxNode is ClassDeclarationSyntax classNode
		&& classNode.TypeParameterList is null
		&& classNode.IsConcrete();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static DeclaredRoslynjectModule SyntaxNodeTransformer(
		GeneratorAttributeSyntaxContext context,
		CancellationToken token)
	{
		var symbol = (INamedTypeSymbol)context.TargetSymbol;
		(string? namespaceName, string className) = symbol.GetNamespaceAndName(token);
		throw new NotImplementedException();
	}


}