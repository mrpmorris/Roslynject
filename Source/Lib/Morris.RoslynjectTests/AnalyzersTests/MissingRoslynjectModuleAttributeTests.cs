//using Morris.Roslynject.Analyzers;
//using Verify = Microsoft.CodeAnalysis.CSharp.Testing.CSharpAnalyzerVerifier<Morris.Roslynject.Analyzers.RoslynjectAttributeAnalyzer, Microsoft.CodeAnalysis.Testing.DefaultVerifier>;


//namespace Morris.RoslynjectTests.AnalyzersTests
//{
//	[TestClass]
//	public class RoslynjectAttributeAnalyzerTests
//	{
//		private static readonly Lazy<string> SharedSource;

//		static RoslynjectAttributeAnalyzerTests()
//		{
//			SharedSource = new Lazy<string>(() =>
//				ReadContents("Morris.RoslynjectTests.StaticResources.cs")
//			);
//		}

//		private static string ReadContents(string name)
//		{
//			using Stream stream = typeof(RoslynjectAttributeAnalyzerTests).Assembly.GetManifestResourceStream(name)!;
//			using var reader = new StreamReader(stream);
//			return reader.ReadToEnd();
//		}

//		[TestMethod]
//		public async Task ClassWithRoslynjectAttributeAndWithoutModuleAttribute_ShouldTriggerDiagnostic()
//		{
//			string testCode =
//				$$"""
//				using Morris.Roslynject;
//				{{ SharedSource.Value }}

//				namespace SourceCode
//				{
//					[Roslynject(Find.DescendantsOf, typeof(BaseClass), Register.BaseType, WithLifetime.Scoped)]
//					public partial class MyModule
//					{

//					}

//					public class BaseClass { }
//					public class Child1 : BaseClass { }
//					public class Child2 : BaseClass { }
//				}
//				""";

//			var expected = Verify
//				.Diagnostic(RoslynjectAttributeAnalyzer.DiagnosticId)
//				.WithSpan(4, 17, 4, 26) // Line and column numbers where diagnostic should be reported.
//				.WithArguments("Class decorated with RoslynjectAttribute should also be decorated with RoslynjectModuleAttribute");

//			await Verify.VerifyAnalyzerAsync(testCode, expected);
//		}

//		[TestMethod]
//		public async Task ClassWithBothRoslynjectAndModuleAttributes_ShouldNotTriggerDiagnostic()
//		{
//			string testCode =
//				$$"""
//				using Morris.Roslynject;
//				{{SharedSource.Value}}

//				namespace SourceCode
//				{
//					[RoslynjectModule(ClassRegex = "Eggs")]
//					[Roslynject(Find.DescendantsOf, typeof(BaseClass), Register.BaseType, WithLifetime.Scoped)]
//					public partial class MyModule
//					{

//					}

//					public class BaseClass { }
//					public class Child1 : BaseClass { }
//					public class Child2 : BaseClass { }
//				}
//				""";

//			await Verify.VerifyAnalyzerAsync(testCode);
//		}

//		[TestMethod]
//		public async Task ClassWithoutRoslynjectAttribute_ShouldNotTriggerDiagnostic()
//		{
//			string testCode =
//				$$"""
//				using Morris.Roslynject;
//				{{SharedSource.Value}}

//				namespace SourceCode
//				{
//					public partial class MyModule
//					{
//					}
//				}
//				""";

//			await Verify.VerifyAnalyzerAsync(testCode);
//		}
//	}

//}
