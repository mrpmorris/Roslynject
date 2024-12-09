using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Morris.Roslynject.Extensions;
using System.CodeDom.Compiler;
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
				transform: TransformRoslynjectAttributeNode
			);

		var injectionCandidates =
			context
			.SyntaxProvider
			.CreateSyntaxProvider(
				predicate: static (x, _) => x is ClassDeclarationSyntax c && c.IsConcrete(),
				transform: TransformInjectionCandidateNode
			)
			.Collect();

		var combined =
			injectionAttributes
			.Combine(injectionCandidates);

		context.RegisterSourceOutput(combined, static (context, items) =>
		{
			var attributes = items.Left;
			var candidates = items.Right;

			using var output = new StringWriter();
			using var codeWriter = new IndentedTextWriter(output);
			codeWriter.WriteLine($"// {DateTime.UtcNow}");
			foreach (KeyValuePair<INamedTypeSymbol, RoslynjectAttributeData> targetAndAttribute in attributes)
			{
				RoslynjectAttributeData attribute = targetAndAttribute.Value;
				codeWriter.Write("//"
					+ $"Find {attribute.Find}"
					+ $", Type {attribute.Type.ToDisplayString()}"
					+ $", Register {attribute.Register}"
					+ $", WithLifetime {attribute.WithLifetime}"
				);
				if (attribute.ClassRegex is not null)
					codeWriter.Write($", ClassRegex {attribute.ClassRegex}");
				if (attribute.ServiceKeyRegex is not null)
					codeWriter.Write($", ServiceKeyRegex {attribute.ServiceKeyRegex}");
				codeWriter.WriteLine();
				GenerateRegistrations(codeWriter, candidates, targetAndAttribute);
			}

			codeWriter.Flush();
			context.AddSource("Morris.Roslynject.g.cs", output.ToString());
		});
	}

	private static ImmutableArray<KeyValuePair<INamedTypeSymbol, RoslynjectAttributeData>> TransformRoslynjectAttributeNode(
		GeneratorAttributeSyntaxContext context,
		CancellationToken token)
	=>
		context
		.Attributes
		.Select(x =>
			new KeyValuePair<INamedTypeSymbol, RoslynjectAttributeData>(
				(INamedTypeSymbol)context.TargetSymbol,
				new RoslynjectAttributeData(x.GetArguments())
			)
		)
		.ToImmutableArray();

	private INamedTypeSymbol TransformInjectionCandidateNode(
		GeneratorSyntaxContext context,
		CancellationToken token)
	=>
		(INamedTypeSymbol) context
		.SemanticModel
		.GetDeclaredSymbol((TypeDeclarationSyntax)context.Node, token)!;

	private static void GenerateRegistrations(
		IndentedTextWriter codeWriter,
		ImmutableArray<INamedTypeSymbol> candidates,
		KeyValuePair<INamedTypeSymbol, RoslynjectAttributeData> targetAndAttribute)
	{
		foreach(INamedTypeSymbol candidate in candidates)
		{

		}
	}

}
