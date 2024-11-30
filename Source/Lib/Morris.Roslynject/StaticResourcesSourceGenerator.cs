using Microsoft.CodeAnalysis;

namespace Morris.Roslynject;

[Generator]
public class StaticResourcesSourceGenerator : IIncrementalGenerator
{
	public readonly static Lazy<string> SourceCode = new(GetStaticResourcesSourceCode);

	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		context.RegisterPostInitializationOutput(ctx =>
		{
			ctx.AddSource("Morris.Roslynject.StaticResources.g.cs", SourceCode.Value);
		});
	}

	private static string GetStaticResourcesSourceCode()
	{
		using Stream stream =
			typeof(StaticResourcesSourceGenerator)
			.Assembly
			.GetManifestResourceStream("Morris.Roslynject.StaticResources.cs");
		using var reader = new StreamReader(stream);
		return reader.ReadToEnd();
	}
}
