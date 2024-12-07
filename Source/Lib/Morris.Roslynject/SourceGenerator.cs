using Microsoft.CodeAnalysis;
using Morris.Roslynject.Extensions;
using Morris.Roslynject.IncrementalValueProviders;
using Morris.Roslynject.IncrementalValueProviders.DeclaredRoslynjectModuleAttributes;
using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Morris.Roslynject;

[Generator]
public class SourceGenerator : IIncrementalGenerator
{
	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		IncrementalValuesProvider<DeclaredRoslynjectModule> roslynjectModulesProvider =
			DeclaredRoslynjectModuleIncrementalValuesProviderFactory.CreateValuesProvider(context);

		IncrementalValuesProvider<INamedTypeSymbol> injectionCandidatesProvider =
			InjectionCandidatesIncrementalValuesProviderFactory.CreateValuesProvider(context);

		var x =
			roslynjectModulesProvider
			.Combine(injectionCandidatesProvider.Collect())
			.Select((x, _) =>
			{
				DeclaredRoslynjectModule left = x.Left;
				ImmutableArray<INamedTypeSymbol> right = x.Right;
				return x;
			});

		context.RegisterSourceOutput(
			source: roslynjectModulesProvider.Collect().Combine(injectionCandidatesProvider.Collect()),
			action: static (productionContext, input) =>
			{
				using var sourceCodeBuilder = new StringWriter();
				using var writer = new IndentedTextWriter(sourceCodeBuilder, tabString: "\t");

				writer.WriteLine("using Microsoft.Extensions.DependencyInjection;");
				// TODO: PeteM - D1
				writer.WriteLine("// " + DateTime.UtcNow.ToString("HH:mm:ss"));

				ImmutableArray<DeclaredRoslynjectModule> roslynjectModules = input.Left;
				ImmutableArray<INamedTypeSymbol> injectionCandidates = input.Right;
				foreach (var roslynjectModule in roslynjectModules)
				{
					writer.AddBlankLine();

					IDisposable? namespaceCodeBlock = null;
					if (!string.IsNullOrEmpty(roslynjectModule.TargetNamespaceName))
					{
						writer.WriteLine($"namespace {roslynjectModule.TargetNamespaceName}");
						namespaceCodeBlock = writer.CodeBlock();
					}

					if (roslynjectModule.ClassRegex is not null)
						writer.WriteLine($"// Only classes matching regex: {roslynjectModule.ClassRegex}");

					writer.WriteLine($"partial class {roslynjectModule.TargetClassName}");
					using (writer.CodeBlock())
					{
						writer.WriteLine("static partial void AfterRegisterServices(IServiceCollection services);");
						writer.AddBlankLine();
						writer.WriteLine("public static void RegisterServices(IServiceCollection services)");
						using (writer.CodeBlock())
						{
							foreach (DeclaredRoslynjectAttribute attr in roslynjectModule.RoslynjectAttributes)
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
