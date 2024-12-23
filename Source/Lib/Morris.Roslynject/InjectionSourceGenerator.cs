using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Morris.Roslynject.Extensions;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading;

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
			var targetsAndAttributes =
				attributes
				.GroupBy(
					keySelector: static x => x.Key,
					elementSelector: static x => x.Value,
					comparer: SymbolEqualityComparer.Default
				);

			using var output = new StringWriter();
			using var writer = new IndentedTextWriter(output);

			//writer.WriteLine($"// {DateTime.UtcNow}");
			writer.WriteLine("using Microsoft.Extensions.DependencyInjection;");

			foreach (var targetAndAttributes in targetsAndAttributes)
			{
				GenerateCodeForTarget(
					context: context,
					candidates: candidates,
					writer: writer,
					target: (INamedTypeSymbol)targetAndAttributes.Key!,
					attributeData: targetAndAttributes);

				writer.AddBlankLine();
			}

			writer.Flush();
			context.AddSource("Morris.Roslynject.g.cs", output.ToString());
		});
	}

	private static void GenerateCodeForTarget(
		SourceProductionContext context,
		ImmutableArray<INamedTypeSymbol> candidates,
		IndentedTextWriter writer,
		INamedTypeSymbol target,
		IEnumerable<RoslynjectAttributeData> attributeData)
	{
		(string? namespaceName, string className) = target.GetNamespaceNameAndClassName(context.CancellationToken);

		IDisposable? namespaceBlock =
			namespaceName is null
			? null
			: writer.CodeBlock($"namespace {namespaceName}");

		using (writer.CodeBlock($"partial class {className}"))
		{
			writer.WriteLine("static partial void AfterRegisterServices(IServiceCollection services);");
			writer.AddBlankLine();
			using (writer.CodeBlock("public static void RegisterServices(IServiceCollection services)"))
			{
				bool addBlankLine = false;
				foreach (RoslynjectAttributeData attributeDatum in attributeData)
				{
					if (addBlankLine)
						writer.AddBlankLine();
					addBlankLine = true;

					writer.Write("//"
						+ $"Find {attributeDatum.Find}"
						+ $", Type {attributeDatum.Type.ToDisplayString()}"
						+ $", Register {attributeDatum.Register}"
						+ $", WithLifetime {attributeDatum.WithLifetime}"
					);
					if (attributeDatum.ClassRegex is not null)
						writer.Write($", ClassRegex {attributeDatum.ClassRegex}");
					if (attributeDatum.ServiceKeyRegex is not null)
						writer.Write($", ServiceKeyRegex {attributeDatum.ServiceKeyRegex}");
					writer.AddBlankLine();

					GenerateRegistrations(writer, candidates, attributeDatum);
				}
				writer.AddBlankLine();
				writer.WriteLine("AfterRegisterServices(services);");
			}
		}

		namespaceBlock?.Dispose();
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
		(INamedTypeSymbol)context
		.SemanticModel
		.GetDeclaredSymbol((TypeDeclarationSyntax)context.Node, token)!;

	private static void GenerateRegistrations(
		IndentedTextWriter codeWriter,
		ImmutableArray<INamedTypeSymbol> candidates,
		RoslynjectAttributeData attributeDatum)
	{
		foreach (INamedTypeSymbol candidate in candidates)
		{
			IEnumerable<KeyValuePair<INamedTypeSymbol, INamedTypeSymbol>> registrationDetails =
				GetRegistrationDetails(
					attributeDatum: attributeDatum,
					candidate: candidate
				);

			foreach(var registration in registrationDetails)
			{
				codeWriter.WriteLine(
					// Lifetime
					$"services.Add{attributeDatum.WithLifetime}("
					// Service key
					+ $"typeof({registration.Key.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)})"
					// Service class
					+ $", typeof({registration.Value.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)}));"
				);
			}
		}
	}

	private static IEnumerable<KeyValuePair<INamedTypeSymbol, INamedTypeSymbol>> GetRegistrationDetails(
		RoslynjectAttributeData attributeDatum,
		INamedTypeSymbol candidate)
	{
		IEnumerable<INamedTypeSymbol> potentialKeys =
			attributeDatum.Type.TypeKind == TypeKind.Interface
			? candidate.Interfaces
			: [candidate];

		Func<INamedTypeSymbol, bool> isMatch = attributeDatum.Find switch {
			Find.DescendantsOf => t => t.DescendsFrom(attributeDatum.Type),
			Find.Exactly => t => SymbolEqualityComparer.Default.Equals(t, attributeDatum.Type),
			Find.AnyTypeOf => t => t.IsTypeOf(attributeDatum.Type),
			_ => throw new NotImplementedException(attributeDatum.Find.ToString())
		};

		Func<INamedTypeSymbol, INamedTypeSymbol> getServiceKey = attributeDatum.Register switch {
			Register.BaseType => _ => attributeDatum.Type,
			Register.DiscoveredInterfaces => _ => throw new NotImplementedException(),
			Register.DiscoveredClasses => _ => candidate,
			Register.BaseClosedGenericType => _ => throw new NotImplementedException(),
			_ => throw new NotImplementedException(attributeDatum.Register.ToString())
		};

		foreach(INamedTypeSymbol potentialKey in potentialKeys)
		{
			if (isMatch(potentialKey))
			{
				INamedTypeSymbol serviceKey = getServiceKey(potentialKey);
				yield return new KeyValuePair<INamedTypeSymbol, INamedTypeSymbol>(serviceKey, candidate);
			}
		}
	}
}
