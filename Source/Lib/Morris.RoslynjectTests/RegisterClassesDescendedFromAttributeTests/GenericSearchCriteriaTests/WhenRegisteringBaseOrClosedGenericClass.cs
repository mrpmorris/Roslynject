//using Morris.Roslynject;

//namespace Morris.RoslynjectTests.RegisterClassesDescendedFromAttributeTests.GenericSearchCriteriaTests;

//[TestClass]
//public class WhenRegisteringBaseOrClosedGenericClass
//{
//	[TestMethod]
//	public void ThenRegistersBaseClosedGenericClassForEachDescendantClass()
//	{
//		const string sourceCode =
//			"""
//			using Microsoft.Extensions.DependencyInjection;
//			using Morris.Roslynject;
//			using System.Collections.Generic;
//			namespace MyNamespace;
			
//			[RegisterClassesDescendedFrom(typeof(List<>), ServiceLifetime.Scoped, RegisterClassAs.BaseOrClosedGenericClass)]
//			internal class MyModule : RoslynjectModule
//			{
//			}
			
//			public class Child1 : List<int> {}
//			public class Child2<T> : List<T> {}
//			public class Child2Child1 : Child2<string> {}
//			""";

//		const string expectedGeneratedCode =
//			"""
//			using Microsoft.Extensions.DependencyInjection;

//			namespace MyNamespace
//			{
//				partial class MyModule
//				{
//					static partial void AfterRegister(IServiceCollection services);

//					public static void Register(IServiceCollection services)
//					{
//						// RegisterClassesDescendedFrom(typeof(List<>), ServiceLifetime.Scoped, RegisterClassAs.BaseOrClosedGenericClass)
//						services.AddScoped(typeof(global::System.Collections.Generic.List<int>), typeof(global::MyNamespace.Child1));
//						services.AddScoped(typeof(global::System.Collections.Generic.List<string>), typeof(global::MyNamespace.Child2Child1));

//						AfterRegister(services);
//					}
//				}
//			}

//			""";

//		SourceGeneratorExecutor
//			.AssertGeneratedCodeMatches(
//				sourceCode: sourceCode,
//				expectedGeneratedCode: expectedGeneratedCode
//			);
//	}

//	[TestMethod]
//	public void ThenRegistersBaseClassForEachClassMatchingRegex()
//	{
//		const string sourceCode =
//			"""
//			namespace MyNamespace
//			{
//				using Microsoft.Extensions.DependencyInjection;
//				using Morris.Roslynject;

//				[RegisterClassesDescendedFrom(typeof(BaseClass), ServiceLifetime.Scoped, RegisterClassAs.BaseOrClosedGenericClass, ClassRegex=@"^MyOtherNamespace\.")]
//				internal class MyModule : RoslynjectModule
//				{
//				}

//				public abstract class BaseClass {}
//				public class Child1<T> : BaseClass {}
//				public class Child2<T> : BaseClass {}
//				public class Child1Child1 : Child1<int> {}
//			}

//			namespace MyOtherNamespace
//			{
//				using Microsoft.Extensions.DependencyInjection;
//				using Morris.Roslynject;
//				using MyNamespace;

//				public class Child1Child1Child1 : Child1Child1 {}
//			}
//			""";

//		const string expectedGeneratedCode =
//			"""
//			using Microsoft.Extensions.DependencyInjection;

//			namespace MyNamespace
//			{
//				partial class MyModule
//				{
//					static partial void AfterRegister(IServiceCollection services);

//					public static void Register(IServiceCollection services)
//					{
//						// RegisterClassesDescendedFrom(typeof(BaseClass), ServiceLifetime.Scoped, RegisterClassAs.BaseOrClosedGenericClass, ClassRegex=@"^MyOtherNamespace\.")
//						services.AddScoped(typeof(global::MyNamespace.BaseClass), typeof(global::MyOtherNamespace.Child1Child1Child1));

//						AfterRegister(services);
//					}
//				}
//			}

//			""";

//		SourceGeneratorExecutor
//			.AssertGeneratedCodeMatches(
//				sourceCode: sourceCode,
//				expectedGeneratedCode: expectedGeneratedCode
//			);
//	}

//}
