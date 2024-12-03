using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using System.Runtime.CompilerServices;
using Morris.Roslynject.Extensions;
using Morris.Roslynject.Generator.Extensions;
using System.Collections.Immutable;

namespace Morris.Roslynject.IncrementalValueProviders.DeclaredRoslynjectModules;

internal class DeclaredRoslynjectModuleIncrementalValuesProviderFactory
{
	public static IncrementalValuesProvider<DeclaredRoslynjectModuleAttribute> CreateValuesProvider(
		IncrementalGeneratorInitializationContext context)
	=>
		context
		.SyntaxProvider
		.ForAttributeWithMetadataName(
			typeof(RoslynjectModuleAttribute).FullName,
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
	private static DeclaredRoslynjectModuleAttribute SyntaxNodeTransformer(
		GeneratorAttributeSyntaxContext context,
		CancellationToken token)
	{
		var symbol = (INamedTypeSymbol)context.TargetSymbol;
		(string? namespaceName, string className) = symbol.GetNamespaceAndName(token);
		AttributeData attribute = context.Attributes[0];
		ImmutableDictionary<string, object?> attributeArgs = attribute.GetArguments();
		var classRegex = (string?)attributeArgs["ClassRegex"];
		var result = new DeclaredRoslynjectModuleAttribute(
			targetNamespaceName: namespaceName,
			targetClassName: className,
			classRegex: classRegex);
		return result;
	}


}