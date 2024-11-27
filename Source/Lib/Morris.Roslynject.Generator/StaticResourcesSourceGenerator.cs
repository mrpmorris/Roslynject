using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Morris.Roslynject.Generator;

[Generator]
public class StaticResourcesSourceGenerator : IIncrementalGenerator
{
	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		// Step 1: Get all additional .cs files
		var additionalFiles = context.AdditionalTextsProvider
			.Where(file => file.Path.EndsWith(".cs", StringComparison.OrdinalIgnoreCase));

		// Step 2: Read the contents of the additional files
		var filesWithContent = additionalFiles
			.Select((file, _) => (Path: file.Path, Content: file.GetText()?.ToString()));

		// Step 3: Register the source output to add the files to the compilation
		context.RegisterSourceOutput(filesWithContent, (spc, item) =>
		{
			if (!string.IsNullOrEmpty(item.Content))
			{
				// Get a valid file name for the generated source
				var fileName = Path.GetFileName(item.Path);

				// Add the source code to the compilation
				spc.AddSource($"Morris.Roslynject.{fileName}", item.Content!);
			}
		});
	}
}
