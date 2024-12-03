using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Morris.Roslynject.Extensions;

namespace Morris.Roslynject.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class RoslynjectAttributeAnalyzer : DiagnosticAnalyzer
{
	public const string DiagnosticId = "RoslynjectModuleMissing";
	private static readonly LocalizableString Title = "RoslynjectModule attribute is missing";
	private static readonly LocalizableString MessageFormat = "Class decorated with RoslynjectAttribute should also be decorated with RoslynjectModuleAttribute";
	private static readonly LocalizableString Description = "All classes with RoslynjectAttribute must also have RoslynjectModuleAttribute.";
	private const string Category = "Usage";

	private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
		DiagnosticId,
		Title,
		MessageFormat,
		Category,
		DiagnosticSeverity.Warning,
		isEnabledByDefault: true,
		description: Description);

	public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [Rule];

	public override void Initialize(AnalysisContext context)
	{
		context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
		context.EnableConcurrentExecution();
		context.RegisterSyntaxNodeAction(AnalyzeClassDeclaration, SyntaxKind.ClassDeclaration);
	}

	private static void AnalyzeClassDeclaration(SyntaxNodeAnalysisContext context)
	{
		var classDeclaration = (ClassDeclarationSyntax)context.Node;

		IEnumerable<AttributeSyntax> attributes = classDeclaration
			.AttributeLists
			.SelectMany(list => list.Attributes);

		bool hasRoslynjectAttribute =
			attributes
			.Any(x => x
				.IsTypeOf(
					context.SemanticModel,
					typeof(RoslynjectAttribute).FullName
				)
			);

		bool hasModuleAttribute =
			attributes
			.Any(x => x
				.IsTypeOf(
					context.SemanticModel,
					typeof(RoslynjectModuleAttribute).FullName
				)
			);

		if (hasRoslynjectAttribute && !hasModuleAttribute)
		{
			var diagnostic = Diagnostic.Create(Rule, classDeclaration.Identifier.GetLocation());
			context.ReportDiagnostic(diagnostic);
		}
	}
}
