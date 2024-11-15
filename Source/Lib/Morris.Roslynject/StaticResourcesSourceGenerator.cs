using Microsoft.CodeAnalysis;
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
			const string SourceCode =
			$$$"""
			using Microsoft.Extensions.DependencyInjection;
			using System.Diagnostics.CodeAnalysis;

			namespace Morris.Roslynject
			{
			}
			""";
			ctx.AddSource("Morris.Roslynject.StaticResources.g.cs", SourceCode);
		});
	}
}
