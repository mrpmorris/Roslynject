using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Morris.Roslynject.Generator;

namespace Morris.RoslynjectTests.RegisterClassesDescendedFromAttributeTests;

[TestClass]
public class WhenRegisteringNonGenericBaseType
{
	[TestMethod]
	public async Task ThenRegistersBaseClassForEachDescendantClass()
	{
		var syntaxTree = CSharpSyntaxTree.ParseText("struct Test {}");
		var compilation = CSharpCompilation.Create(
			assemblyName: "Test",
			syntaxTrees: [syntaxTree],
			references: Basic.Reference.Assemblies.Net80.References.All,
			options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
		);

		var subject = new RoslynjectGenerator();
		ISourceGenerator sourceGenerator = subject.AsSourceGenerator();
		var driver = CSharpGeneratorDriver
			.Create(sourceGenerator)
			.RunGenerators(compilation);

		// Assert the driver doesn't recompute the output
		GeneratorRunResult result = driver.GetRunResult().Results.Single();
		//var allOutputs = result.TrackedOutputSteps.SelectMany(outputStep => outputStep.Value).SelectMany(output => output.Outputs);
		//Assert.Collection(allOutputs, output => Assert.Equal(IncrementalStepRunReason.Cached, output.Reason));

		//// Assert the driver use the cached result from AssemblyName and Syntax
		//var assemblyNameOutputs = result.TrackedSteps["AssemblyName"].Single().Outputs;
		//Assert.Collection(assemblyNameOutputs, output => Assert.Equal(IncrementalStepRunReason.Unchanged, output.Reason));

		//var syntaxOutputs = result.TrackedSteps["Syntax"].Single().Outputs;
		//Assert.Collection(syntaxOutputs, output => Assert.Equal(IncrementalStepRunReason.Unchanged, output.Reason));


		await Task.Yield();
	}

}
