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
			foreach (var attribute in attributes)
			{
				ImmutableDictionary<string, object?> dictionary = attribute.Value;
				Find find = dictionary.GetValue<Find>(nameof(RoslynjectAttribute.Find));
				INamedTypeSymbol type = dictionary.GetValue<INamedTypeSymbol>(nameof(RoslynjectAttribute.Type));
				Register register = dictionary.GetValue<Register>(nameof(RoslynjectAttribute.Register));
				WithLifetime withLifetime = dictionary.GetValue<WithLifetime>(nameof(RoslynjectAttribute.WithLifetime));
				string? classRegex = dictionary.GetValueOrDefault<string?>(nameof(RoslynjectAttribute.ClassRegex));
				string? serviceKeyRegex = dictionary.GetValueOrDefault<string?>(nameof(RoslynjectAttribute.ServiceKeyRegex));
				codeWriter.Write("//"
					+ $"Find {find}"
					+ $", Type {type.ToDisplayString()}"
					+ $", Register {register}"
					+ $", WithLifetime {withLifetime}"
				);
				if (classRegex is not null)
					codeWriter.Write($", ClassRegex {classRegex}");
				if (serviceKeyRegex is not null)
					codeWriter.Write($", ServiceKeyRegex {serviceKeyRegex}");
				codeWriter.WriteLine();
				GenerateRegistrations(codeWriter, candidates, attribute);
			}

			codeWriter.Flush();
			context.AddSource("Morris.Roslynject.g.cs", output.ToString());
		});
	}

	private static ImmutableArray<KeyValuePair<INamedTypeSymbol, ImmutableDictionary<string, object?>>> TransformRoslynjectAttributeNode(
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
		KeyValuePair<INamedTypeSymbol, ImmutableDictionary<string, object>> attribute)
	{
		throw new NotImplementedException();
	}

}
