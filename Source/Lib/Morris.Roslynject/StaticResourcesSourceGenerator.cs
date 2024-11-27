using Microsoft.CodeAnalysis;

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
					public class MyAttribute : Attribute
					{
						public string Name { get; set; }

						public MyAttribute(string name)
						{
							Name = name;
						}
					}
				}
				""";
			ctx.AddSource("Morris.Roslynject.StaticResources.g.cs", sourceCode);
		});
	}
}
