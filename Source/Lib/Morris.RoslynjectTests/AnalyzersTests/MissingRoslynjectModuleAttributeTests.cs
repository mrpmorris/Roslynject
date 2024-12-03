using Morris.Roslynject.Analyzers;
using Verify = Microsoft.CodeAnalysis.CSharp.Testing.CSharpAnalyzerVerifier<Morris.Roslynject.Analyzers.RoslynjectAttributeAnalyzer, Microsoft.CodeAnalysis.Testing.DefaultVerifier>;


namespace Morris.RoslynjectTests.AnalyzersTests
{
	[TestClass]
	public class RoslynjectAttributeAnalyzerTests
	{
		[TestMethod]
		public async Task ClassWithRoslynjectAttributeAndWithoutModuleAttribute_ShouldTriggerDiagnostic()
		{
			string testCode = 
				"""
				using Morris.Roslynject;

				[Roslynject]
				public class TestClass
				{
				}
				""";

			var expected = Verify
				.Diagnostic(RoslynjectAttributeAnalyzer.DiagnosticId)
				.WithSpan(4, 17, 4, 26) // Line and column numbers where diagnostic should be reported.
				.WithArguments("Class decorated with RoslynjectAttribute should also be decorated with RoslynjectModuleAttribute");

			await Verify.VerifyAnalyzerAsync(testCode, expected);
		}

		[TestMethod]
		public async Task ClassWithBothRoslynjectAndModuleAttributes_ShouldNotTriggerDiagnostic()
		{
			string testCode = 
				"""
				using Morris.Roslynject;

				[Roslynject]
				[RoslynjectModule]
				public class TestClass
				{
				}
				""";

			await Verify.VerifyAnalyzerAsync(testCode);
		}

		[TestMethod]
		public async Task ClassWithoutRoslynjectAttribute_ShouldNotTriggerDiagnostic()
		{
			string testCode =
				"""
				public class TestClass
				{
				}
				""";

			await Verify.VerifyAnalyzerAsync(testCode);
		}
	}
}
