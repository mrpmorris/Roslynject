using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace Morris.Roslynject;

[Generator]
public class MySourceGenerator : IIncrementalGenerator
{
	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		// Collect all syntax nodes that look like class declarations
		var classDeclarations = context.SyntaxProvider
			.CreateSyntaxProvider(
				predicate: IsCandidateClassDeclaration,
				transform: GetSemanticTargetForGeneration)
			.Where(static node => node is not null);

		// Collect the results and pass them for generation
		var compilationAndClasses = context.CompilationProvider.Combine(classDeclarations.Collect());

		context.RegisterSourceOutput(compilationAndClasses, Execute);
	}

	private static bool IsCandidateClassDeclaration(SyntaxNode syntaxNode, CancellationToken cancellationToken)
	{
		return syntaxNode is ClassDeclarationSyntax classDeclaration &&
			   classDeclaration.AttributeLists.Count > 0;
	}

	private static ClassDeclarationSyntax? GetSemanticTargetForGeneration(GeneratorSyntaxContext context, CancellationToken cancellationToken)
	{
		var classDeclaration = (ClassDeclarationSyntax)context.Node;

		// Check if any attributes match "Morris.Roslynject.MyAttribute"
		foreach (var attributeList in classDeclaration.AttributeLists)
		{
			foreach (var attribute in attributeList.Attributes)
			{
				var model = context.SemanticModel;
				var attributeSymbol = model.GetSymbolInfo(attribute, cancellationToken).Symbol as IMethodSymbol;

				if (attributeSymbol is not null &&
					attributeSymbol.ContainingType.ToDisplayString() == "Morris.Roslynject.MyAttribute")
				{
					return classDeclaration;
				}
			}
		}

		return null;
	}

	private static void Execute(SourceProductionContext context, (Compilation Compilation, ImmutableArray<ClassDeclarationSyntax?> Classes) source)
	{
		var (compilation, classes) = source;

		foreach (var classDeclaration in classes.Where(c => c is not null))
		{
			// Generate code based on each matched class
			GenerateCodeForClass(context, compilation, classDeclaration!);
		}
	}

	private static void GenerateCodeForClass(SourceProductionContext context, Compilation compilation, ClassDeclarationSyntax classDeclaration)
	{
		// Use semantic model and class declaration to generate appropriate code.
		// For example, generate a partial class or some helper methods.
		var className = classDeclaration.Identifier.Text;

		// Example source output
		context.AddSource($"{className}_Generated.g.cs", $@"
namespace {compilation.AssemblyName}
{{
	partial class {className}
	{{
		// Generated content
	}}
}}");
	}
}


