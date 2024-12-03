using Microsoft.CodeAnalysis;
using Morris.Roslynject.Extensions;
using Morris.Roslynject.IncrementalValueProviders;
using Morris.Roslynject.IncrementalValueProviders.DeclaredRoslynjectModuleAttributes;
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
							foreach (DeclaredRoslynjectAttribute attr in moduleClass.RoslynjectAttributes)
							{
								writer.WriteLine(
									$"// Find: {attr.Find},"
									+ $" Type: {attr.Type.ToDisplayString()},"
									+ $" Register: {attr.Register},"
									+ $" WithLifetime: {attr.WithLifetime}"
								);
								if (attr.ClassRegex is not null)
									writer.WriteLine($"// ClassRegex: {attr.ClassRegex}");
								if (attr.ServiceKeyRegex is not null)
									writer.WriteLine($"// ServiceKeyRegex: {attr.ServiceKeyRegex}");

								writer.AddBlankLine();
							}
							writer.WriteLine("AfterRegisterServices(services);");
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
