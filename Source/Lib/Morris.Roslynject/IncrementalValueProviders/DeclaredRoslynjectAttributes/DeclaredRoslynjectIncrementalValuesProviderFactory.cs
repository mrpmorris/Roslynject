using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using System.Runtime.CompilerServices;
using Morris.Roslynject.Extensions;
using System.Collections.Immutable;

namespace Morris.Roslynject.IncrementalValueProviders.DeclaredRoslynjectAttributes;

internal class DeclaredRoslynjectIncrementalValuesProviderFactory
{
	public static IncrementalValuesProvider<DeclaredRoslynjectAttribute> CreateValuesProvider(
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
	private static DeclaredRoslynjectAttribute SyntaxNodeTransformer(
		GeneratorAttributeSyntaxContext context,
		CancellationToken token)
	{
		var symbol = (INamedTypeSymbol)context.TargetSymbol;
		(string? namespaceName, string className) = symbol.GetNamespaceAndName(token);
		AttributeData attribute = context.Attributes[0];
		ImmutableDictionary<string, object?> attributeArgs = attribute.GetArguments();
		var find = attributeArgs.GetValue<Find>("find");
		var type = attributeArgs.GetValue<INamedTypeSymbol>("type");
		var register = attributeArgs.GetValue<Register>("register");
		var withLifetime = attributeArgs.GetValue<WithLifetime>("withLifetime");
		var classRegex = attributeArgs.GetValueOrDefault<string?>("classRegex");
		var serviceKeyRegex = attributeArgs.GetValueOrDefault<string?>("serviceKeyRegex");

		var result = new DeclaredRoslynjectAttribute(
			targetFullName: symbol.ToDisplayString(),
			find: find,
			type: type,
			register: register,
			withLifetime: withLifetime,
			classRegex: classRegex,
			serviceKeyRegex: serviceKeyRegex);
		return result;
	}


}