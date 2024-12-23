﻿using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Morris.Roslynject;
using System.Text.RegularExpressions;

namespace Morris.RoslynjectTests;

internal static class SourceGeneratorExecutor
{

	private static readonly MetadataReference MSDependencyInjectionMetadataReference =
		MetadataReference
		.CreateFromFile(
			typeof(ServiceLifetime)
			.Assembly
			.Location
		);

	public static void AssertGeneratedCodeMatches(
		string sourceCode,
		string expectedGeneratedCode)
	{
		var staticResourcesSyntaxTree = CSharpSyntaxTree.ParseText(StaticResourcesSourceGenerator.SourceCode.Value);
		var unitTestSyntaxTree = CSharpSyntaxTree.ParseText(sourceCode);

		var compilation = CSharpCompilation.Create(
			assemblyName: "Test",
			syntaxTrees: [staticResourcesSyntaxTree, unitTestSyntaxTree],
			references: Basic.Reference.Assemblies.Net90.References
				.All
				.Union([MSDependencyInjectionMetadataReference]),
			options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, reportSuppressedDiagnostics: true)
		);

		var subject = new InjectionSourceGenerator().AsSourceGenerator();
		var driver = CSharpGeneratorDriver
			.Create(subject)
			.RunGenerators(compilation);

		GeneratorDriverRunResult runResult = driver.GetRunResult();
		GeneratorRunResult result = runResult.Results.Single();

		GeneratedSourceResult generatedSource = result.GeneratedSources.Single();
		Assert.AreEqual("Morris.Roslynject.g.cs", generatedSource.HintName);
		string generatedCode = generatedSource.SyntaxTree.ToString();
		Assert.AreEqual(
			TidyCode(expectedGeneratedCode),
			TidyCode(generatedCode)
		);
	}

	public static void AssertGeneratedEmptyModule(
		string sourceCode,
		string namespaceName = "MyNamespace",
		string moduleClassName = "MyModule")
	{
		string expectedGeneratedCode =
			$$$"""
			using Microsoft.Extensions.DependencyInjection;

			namespace {{{namespaceName}}}
			{
				partial class {{{moduleClassName}}}
				{
					static partial void AfterRegister(IServiceCollection services);

					public static void Register(IServiceCollection services)
					{
						AfterRegister(services);
					}
				}
			}

			""";
		AssertGeneratedCodeMatches(sourceCode, expectedGeneratedCode);
	}

	private static string TidyCode(string value) =>
		Regex.Replace(
			input: value.Replace("\r", ""),
			pattern: "(?m)^\t+",
			evaluator: x => new string(' ', x.Length * 4)
		)
		.Trim();
}
