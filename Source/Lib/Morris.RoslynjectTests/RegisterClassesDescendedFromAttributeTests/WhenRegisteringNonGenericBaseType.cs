using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Extensions.DependencyInjection;
using Morris.Roslynject.Generator;
using static Basic.Reference.Assemblies.Net80;

namespace Morris.RoslynjectTests.RegisterClassesDescendedFromAttributeTests;

[TestClass]
public class WhenRegisteringNonGenericBaseType
{
	[TestMethod]
	public void ThenRegistersBaseClassForEachDescendantClass()
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(
			$$$"""
			using Microsoft.Extensions.DependencyInjection;
			using Morris.Roslynject;
			namespace MyNamespace;

			[RegisterClassesDescendedFrom(typeof(BaseClass), ServiceLifetime.Scoped, ClassRegistration.BaseClass)]
			internal class MyModule : RoslynjectModule
			{
			}

			public class BaseClass {}
			public class Child1 : BaseClass {}
			public class Child2 : BaseClass {}
			public class Child1Child1 : Child1 {}
			"""
		);

		var morrisRoslynjectAssembly = typeof(Morris.Roslynject.RegisterClassesDescendedFromAttribute).Assembly;
		var morrisRoslynjectMetadataReference = MetadataReference.CreateFromFile(morrisRoslynjectAssembly.Location);

		var injectionAssembly = typeof(ServiceLifetime).Assembly;
		var injectionMetadataReference = MetadataReference.CreateFromFile(injectionAssembly.Location);

		var compilation = CSharpCompilation.Create(
			assemblyName: "Test",
			syntaxTrees: [syntaxTree],
			references: Basic.Reference.Assemblies.Net80.References
				.All
				.Union([morrisRoslynjectMetadataReference, injectionMetadataReference]),
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
		string expectedGeneratedCode =
"""
using Microsoft.Extensions.DependencyInjection;

namespace MyNamespace
{
    partial class MyModule
    {
        static partial void AfterRegister(IServiceCollection services);

        public static void Register(IServiceCollection services)
        {
            // RegisterClassesDescendedFrom(typeof(BaseClass), ServiceLifetime.Scoped, ClassRegistration.BaseClass)
            services.AddScoped(typeof(global::MyNamespace.Child1));
            services.AddScoped(typeof(global::MyNamespace.Child2));
            services.AddScoped(typeof(global::MyNamespace.Child1Child1));

            AfterRegister(services);
        }
    }
}

""".Replace("\r", "");

		Assert.AreEqual(expectedGeneratedCode, generatedCode);
	}

}
