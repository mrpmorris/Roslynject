using Microsoft.CodeAnalysis;
using Morris.Roslynject.StaticResources;
using System;
using System.Collections.Generic;
using System.Text;

namespace Morris.Roslynject;

[Generator]
public class StaticResourcesSourceGenerator : IIncrementalGenerator
{
	public readonly static string SourceCode = GenerateSourceCode();

	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		context.RegisterPostInitializationOutput(ctx =>
		{
			ctx.AddSource("Morris.Roslynject.StaticResources.g.cs", SourceCode);
		});
	}

	private static string GenerateSourceCode() =>
		$$$"""
		using Microsoft.Extensions.DependencyInjection;
		using System;
		using System.Diagnostics.CodeAnalysis;

		namespace Morris.Roslynject
		{
		{{{StaticResources.SourceCode.RoslynjectModule}}}

		{{{StaticResources.SourceCode.RegisterClassAs}}}

		{{{StaticResources.SourceCode.RegisterInterfaceAs}}}

		{{{StaticResources.SourceCode.RegisterClassesDescendedFromAttribute}}}
		}
		""";

}
