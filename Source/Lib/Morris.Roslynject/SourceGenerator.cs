using Microsoft.CodeAnalysis;
using Morris.Roslynject.Extensions;
using Morris.Roslynject.IncrementalValueProviders;
using Morris.Roslynject.IncrementalValueProviders.DeclaredRoslynjectModuleAttributes;
using Morris.Roslynject.IncrementalValueProviders.RoslynjectModules;
using System.CodeDom.Compiler;

namespace Morris.Roslynject;

[Generator]
public class SourceGenerator : IIncrementalGenerator
{
	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		IncrementalValuesProvider<DeclaredRoslynjectModule> declaredModulesProvider =
			DeclaredRoslynjectModuleIncrementalValuesProviderFactory.CreateValuesProvider(context);

		IncrementalValuesProvider<INamedTypeSymbol> injectionCandidatesProvider =
			InjectionCandidatesIncrementalValuesProviderFactory.CreateValuesProvider(context);

		IncrementalValuesProvider<Module> modulesProvider =
			declaredModulesProvider
			.Combine(injectionCandidatesProvider.Collect())
			.Select(static (x, _) => new Module(x.Left, x.Right));

		context.RegisterSourceOutput(
			source: modulesProvider.Collect(),
			action: static (productionContext, input) =>
			{
				using var sourceCodeBuilder = new StringWriter();
				using var writer = new IndentedTextWriter(sourceCodeBuilder, tabString: "\t");

				writer.WriteLine("using Microsoft.Extensions.DependencyInjection;");
				// TODO: PeteM - D1
				writer.WriteLine("// " + DateTime.UtcNow.ToString("HH:mm:ss"));

				foreach (Module module in input)
				{
					DeclaredRoslynjectModule declaredModule = module.DeclaredModule;
					writer.AddBlankLine();

					IDisposable? namespaceCodeBlock = null;
					if (!string.IsNullOrEmpty(declaredModule.TargetNamespaceName))
					{
						writer.WriteLine($"namespace {declaredModule.TargetNamespaceName}");
						namespaceCodeBlock = writer.CodeBlock();
					}

					if (declaredModule.ClassRegex is not null)
						writer.WriteLine($"// Only classes matching regex: {declaredModule.ClassRegex}");

					writer.WriteLine($"partial class {declaredModule.TargetClassName}");
					using (writer.CodeBlock())
					{
						writer.WriteLine("static partial void AfterRegisterServices(IServiceCollection services);");
						writer.AddBlankLine();
						writer.WriteLine("public static void RegisterServices(IServiceCollection services)");
						using (writer.CodeBlock())
						{
							foreach (AttributeAndRegistrations attributeAndRegistrations in module.AttributeAndRegistrations)
							{
								var declaredAttribute = attributeAndRegistrations.DeclaredRoslynjectAttribute;
								writer.WriteLine(
									$"// Find: {declaredAttribute.Find},"
									+ $" Type: {declaredAttribute.Type.ToDisplayString()},"
									+ $" Register: {declaredAttribute.Register},"
									+ $" WithLifetime: {declaredAttribute.WithLifetime}"
								);
								if (declaredAttribute.ClassRegex is not null)
									writer.WriteLine($"// ClassRegex: {declaredAttribute.ClassRegex}");

								foreach (ServiceRegistration registration in attributeAndRegistrations.Registrations)
								{
									writer.WriteLine(
										$"services.Add{registration.WithLifetime}("
										+ $"typeof({registration.ServiceKeyTypeName ?? registration.ServiceTypeName})"
										+ $", typeof({registration.ServiceTypeName}))"
									);
								}

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
