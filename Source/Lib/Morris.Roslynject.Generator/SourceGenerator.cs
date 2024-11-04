using Microsoft.CodeAnalysis;
using Morris.Roslynject.Generator.Extensions;
using Morris.Roslynject.Generator.IncrementalValueProviders.DeclaredRegistrationClasses;
using Morris.Roslynject.Generator.IncrementalValueProviders.InjectionCandidates;
using Morris.Roslynject.Generator.IncrementalValueProviders.RegistrationClassOutputs;
using System.CodeDom.Compiler;

namespace Morris.Roslynject.Generator;

[Generator]
public class RoslynjectGenerator : IIncrementalGenerator
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
                using var writer = new IndentedTextWriter(sourceCodeBuilder);

                // TODO: PeteM - D1
                writer.WriteLine($"// Generated at {DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}");
                writer.WriteLine("using Microsoft.Extensions.DependencyInjection;");

                foreach (var registrationClass in input)
                {
                    writer.WriteLine();

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
                        writer.WriteLine();
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
                                writer.WriteLine();
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
