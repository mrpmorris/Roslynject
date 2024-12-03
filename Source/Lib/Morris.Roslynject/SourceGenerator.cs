using Microsoft.CodeAnalysis;
using Morris.Roslynject.Extensions;
using Morris.Roslynject.IncrementalValueProviders;
using Morris.Roslynject.IncrementalValueProviders.DeclaredRoslynjectModules;
using System.CodeDom.Compiler;

namespace Morris.Roslynject;

[Generator]
public class SourceGenerator : IIncrementalGenerator
{
	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		IncrementalValuesProvider<INamedTypeSymbol> injectionCandidates =
			InjectionCandidatesIncrementalValuesProviderFactory.CreateValuesProvider(context);

		IncrementalValuesProvider<DeclaredRoslynjectModuleAttribute> roslynjectModules =
			DeclaredRoslynjectModuleIncrementalValuesProviderFactory.CreateValuesProvider(context);

		context.RegisterSourceOutput(
			source: roslynjectModules.Collect(),
			action: static (productionContext, input) =>
			{
				using var sourceCodeBuilder = new StringWriter();
				using var writer = new IndentedTextWriter(sourceCodeBuilder, tabString: "\t");

				writer.WriteLine("using Microsoft.Extensions.DependencyInjection;");

				foreach (var moduleClass in input)
				{
					writer.AddBlankLine();

					IDisposable? namespaceCodeBlock = null;
					if (!string.IsNullOrEmpty(moduleClass.TargetNamespaceName))
					{
						writer.WriteLine($"namespace {moduleClass.TargetNamespaceName}");
						namespaceCodeBlock = writer.CodeBlock();
					}

					if (moduleClass.ClassRegex is not null)
						writer.WriteLine($"// Only classes matching regex: {moduleClass.ClassRegex}");

					writer.WriteLine($"partial class {moduleClass.TargetClassName}");
					using (writer.CodeBlock())
					{
						writer.WriteLine("static partial void AfterRegisterServices(IServiceCollection services);");
						writer.AddBlankLine();
						writer.WriteLine("public static void RegisterServices(IServiceCollection services)");
						using (writer.CodeBlock())
						{
							//foreach (RegisterAttributeOutputBase attr in moduleClass.Attributes)
							//{
							//	string attributeSourceCode = attr.AttributeSourceCode
							//		.ToString()
							//		.Replace("\r\n", " ")
							//		.Replace('\n', ' ');
							//	writer.WriteLine($"// {attributeSourceCode}");

							//	attr.GenerateCode(writer.WriteLine);
							//	writer.AddBlankLine();
							//}
							//writer.WriteLine("AfterRegisterServices(services);");
						}
					}
					namespaceCodeBlock?.Dispose();
				}

				writer.Flush();

				string generatedSourceCode = sourceCodeBuilder.ToString();
				productionContext.AddSource("Morris.Roslynject.g.cs", generatedSourceCode);
			});
	}
}
