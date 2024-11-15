using Microsoft.CodeAnalysis;
using Morris.Roslynject.StaticResources;
using System;
using System.Collections.Generic;
using System.Text;

namespace Morris.Roslynject;

[Generator]
public class StaticResourcesSourceGenerator : IIncrementalGenerator
{
	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		context.RegisterPostInitializationOutput(ctx =>
		{
			string sourceCode =
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
			ctx.AddSource("Morris.Roslynject.StaticResources.g.cs", sourceCode);
		});
	}
}
