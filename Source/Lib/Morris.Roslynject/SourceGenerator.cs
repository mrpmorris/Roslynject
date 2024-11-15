using Microsoft.CodeAnalysis;
using Morris.Roslynject.Extensions;
using Morris.Roslynject.IncrementalValueProviders.DeclaredRegistrationClasses;
using Morris.Roslynject.IncrementalValueProviders.InjectionCandidates;
using Morris.Roslynject.IncrementalValueProviders.RegistrationClassOutputs;
using System.CodeDom.Compiler;

namespace Morris.Roslynject;

[Generator]
public class SourceGenerator : IIncrementalGenerator
{
	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		IncrementalValuesProvider<DeclaredRegistrationClass> declaredRegistrationClasses =
			DeclaredRegistrationClassesFactory.CreateValuesProvider(context);

		IncrementalValuesProvider<INamedTypeSymbol> injectionCandidates =
			InjectionCandidatesFactory.CreateValuesProvider(context);

		IncrementalValuesProvider<RegistrationClassOutput> registrationClassOutputs =
			RegistrationClassOutputsFactory.CreateValuesProvider(
				declaredRegistrationClasses: declaredRegistrationClasses,
				injectionCandidates: injectionCandidates);

		context.RegisterSourceOutput(
			source: registrationClassOutputs.Collect(),
			static (productionContext, input) =>
			{
				using var sourceCodeBuilder = new StringWriter();
				using var writer = new IndentedTextWriter(sourceCodeBuilder, tabString: "\t");

				writer.WriteLine("using Microsoft.Extensions.DependencyInjection;");

				foreach (var registrationClass in input)
				{
					writer.AddBlankLine();

					IDisposable? namespaceCodeBlock = null;
					if (!string.IsNullOrEmpty(registrationClass.NamespaceName))
					{
						writer.WriteLine($"namespace {registrationClass.NamespaceName}");
						namespaceCodeBlock = writer.CodeBlock();
					}

					writer.WriteLine($"partial class {registrationClass.ClassName}");
					using (writer.CodeBlock())
					{
						writer.WriteLine("static partial void AfterRegister(IServiceCollection services);");
						writer.AddBlankLine();
						writer.WriteLine("public static void Register(IServiceCollection services)");
						using (writer.CodeBlock())
						{
							foreach (RegisterAttributeOutputBase attr in registrationClass.Attributes)
							{
								string attributeSourceCode = attr.AttributeSourceCode
									.ToString()
									.Replace("\r\n", " ")
									.Replace('\n', ' ');
								writer.WriteLine($"// {attributeSourceCode}");

								attr.GenerateCode(writer.WriteLine);
								writer.AddBlankLine();
							}
							writer.WriteLine("AfterRegister(services);");
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
