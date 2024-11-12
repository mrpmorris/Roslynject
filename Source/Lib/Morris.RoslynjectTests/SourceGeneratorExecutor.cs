using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Morris.Roslynject.Generator;
using System.Diagnostics.CodeAnalysis;

namespace Morris.RoslynjectTests;
internal static class SourceGeneratorExecutor
{
	private static readonly MetadataReference MorrisRoslynjectMetadataReference =
		MetadataReference
		.CreateFromFile(
			typeof(Morris.Roslynject.RegisterClassesDescendedFromAttribute)
			.Assembly
			.Location
		);

	private static readonly MetadataReference MSDependencyInjectionMetadataReference =
		MetadataReference
		.CreateFromFile(
			typeof(ServiceLifetime)
			.Assembly
			.Location
		);

	public static string AssertGeneratedCodeMatches(
		string sourceCode,
		string expectedGeneratedCode)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);

		var compilation = CSharpCompilation.Create(
			assemblyName: "Test",
			syntaxTrees: [syntaxTree],
			references: Basic.Reference.Assemblies.Net80.References
				.All
				.Union([MorrisRoslynjectMetadataReference, MSDependencyInjectionMetadataReference]),
			options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
		);

		var subject = new SourceGenerator();
		ISourceGenerator sourceGenerator = subject.AsSourceGenerator();
		var driver = CSharpGeneratorDriver
			.Create(sourceGenerator)
			.RunGenerators(compilation);

		GeneratorDriverRunResult runResult = driver.GetRunResult();
		GeneratorRunResult result = runResult.Results.Single();

		GeneratedSourceResult generatedSource = result.GeneratedSources.Single();
		Assert.AreEqual("Morris.Roslynject.g.cs", generatedSource.HintName);
		string generatedCode = generatedSource.SyntaxTree.ToString().Replace("\r", "");
		Assert.AreEqual(expectedGeneratedCode.Replace("\r", ""), generatedCode);

		return generatedCode;
	}
}
