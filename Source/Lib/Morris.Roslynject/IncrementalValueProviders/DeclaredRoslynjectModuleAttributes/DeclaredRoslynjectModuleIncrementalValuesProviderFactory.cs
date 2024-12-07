using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using System.Runtime.CompilerServices;
using Morris.Roslynject.Extensions;
using System.Collections.Immutable;

namespace Morris.Roslynject.IncrementalValueProviders.DeclaredRoslynjectModuleAttributes;

internal class DeclaredRoslynjectModuleIncrementalValuesProviderFactory
{
	public static IncrementalValuesProvider<DeclaredRoslynjectModule> CreateValuesProvider(
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
	private static DeclaredRoslynjectModule SyntaxNodeTransformer(
		GeneratorAttributeSyntaxContext context,
		CancellationToken token)
	{
		var symbol = (INamedTypeSymbol)context.TargetSymbol;

		AttributeData attribute = context.Attributes[0];
		ImmutableDictionary<string, object?> attributeArgs = attribute.GetArguments();

		(string? namespaceName, string className) = symbol.GetNamespaceAndName(token);
		var classRegex = (string?)attributeArgs["ClassRegex"];
		var result = new DeclaredRoslynjectModule(
			targetNamespaceName: namespaceName,
			targetClassName: className,
			classRegex: classRegex,
			roslynjectAttributes: FindRoslynjectAttributes(context.SemanticModel, symbol)
		);
		return result;
	}

	private static IEnumerable<DeclaredRoslynjectAttribute> FindRoslynjectAttributes(
		SemanticModel semanticModel,
		INamedTypeSymbol symbol)
	{
		INamedTypeSymbol roslynjectSymbolAttribute =
				semanticModel
				.Compilation
				.GetTypeByMetadataName(typeof(RoslynjectAttribute).FullName)!;

		IEnumerable<AttributeData> attributes =
			symbol
			.GetAttributes()
			.Where(x => SymbolEqualityComparer.Default.Equals(x.AttributeClass, roslynjectSymbolAttribute));

		foreach(AttributeData attribute in attributes)
		{
			ImmutableDictionary<string, object?> attributeArgs = attribute.GetArguments();
			Find find = attributeArgs.GetValue<Find>("Find");
			INamedTypeSymbol type = attributeArgs.GetValue<INamedTypeSymbol>("Type");
			Register register = attributeArgs.GetValue<Register>("Register");
			WithLifetime withLifetime = attributeArgs.GetValue<WithLifetime>("WithLifetime");
			string? classRegex = attributeArgs.GetValueOrDefault<string?>("ClassRegex");
			string? serviceKeyRegex = attributeArgs.GetValueOrDefault<string?>("ServiceKeyRegex");

			yield return new DeclaredRoslynjectAttribute(
				find: find,
				type: type,
				register: register,
				withLifetime: withLifetime,
				classRegex: classRegex,
				serviceKeyRegex: serviceKeyRegex);
		}
	}
}