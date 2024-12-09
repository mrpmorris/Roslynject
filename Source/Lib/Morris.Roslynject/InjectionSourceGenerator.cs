using Microsoft.CodeAnalysis;
using Morris.Roslynject.Extensions;
using System.Collections.Immutable;

namespace Morris.Roslynject;

[Generator]
public class InjectionSourceGenerator : IIncrementalGenerator
{
	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		var injectionAttributes =
			context
			.SyntaxProvider
			.ForAttributeWithMetadataName(
				fullyQualifiedMetadataName: "Morris.Roslynject.RoslynjectAttribute",
				predicate: static (_, _) => true,
				transform: TransformNode
			);


		context.RegisterSourceOutput(injectionAttributes, static (context, item) =>
		{
			throw new NotImplementedException();
		});
	}

	private static ImmutableArray<KeyValuePair<INamedTypeSymbol, ImmutableDictionary<string, object?>>> TransformNode(
		GeneratorAttributeSyntaxContext context,
		CancellationToken token)
	=>
		context
		.Attributes
		.Select(x =>
			new KeyValuePair<INamedTypeSymbol, ImmutableDictionary<string, object?>>(
				(INamedTypeSymbol)context.TargetSymbol,
				x.GetArguments()
			)
		)
		.ToImmutableArray();

}
