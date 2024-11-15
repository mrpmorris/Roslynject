using Morris.Roslynject;

namespace Morris.RoslynjectTests.RegisterClassesDescendedFromAttributeTests.GenericSearchCriteriaTests;

[TestClass]
public class WhenRegisteringBaseOrClosedGenericClass
{
	[TestMethod]
	public void ThenRegistersBaseClosedGenericClassForEachDescendantClass()
	{
		const string sourceCode =
			"""
			using Microsoft.Extensions.DependencyInjection;
			using Morris.Roslynject;
			namespace MyNamespace;
			
			[RegisterClassesDescendedFrom(typeof(BaseClass<,>), ServiceLifetime.Scoped, RegisterClassAs.BaseOrClosedGenericClass)]
			internal class MyModule : RoslynjectModule
			{
			}

			public class BaseClass<TRequest, TResponse> {}
			public class Child1 : BaseClass<int, bool> {}
			public class Child2<TRequest> : BaseClass<TRequest, bool> {}
			public class Child2Child1 : Child2<string> {}
			""";

		const string expectedGeneratedCode =
			"""
			using Microsoft.Extensions.DependencyInjection;

			namespace MyNamespace
			{
				partial class MyModule
				{
					static partial void AfterRegister(IServiceCollection services);

					public static void Register(IServiceCollection services)
					{
						// RegisterClassesDescendedFrom(typeof(BaseClass<,>), ServiceLifetime.Scoped, RegisterClassAs.BaseOrClosedGenericClass)
						services.AddScoped(typeof(global::MyNamespace.BaseClass<int, bool>), typeof(global::MyNamespace.Child1));
						services.AddScoped(typeof(global::MyNamespace.BaseClass<string, bool>), typeof(global::MyNamespace.Child2Child1));

						AfterRegister(services);
					}
				}
			}

			""";

		SourceGeneratorExecutor
			.AssertGeneratedCodeMatches(
				sourceCode: sourceCode,
				expectedGeneratedCode: expectedGeneratedCode
			);
	}

	//[TestMethod]
	//public void ThenRegistersBaseClassForEachClassMatchingRegex()
	//{
	//	const string sourceCode =
	//		"""
	//		namespace MyNamespace
	//		{
	//			using Microsoft.Extensions.DependencyInjection;
	//			using Morris.Roslynject;

	//			[RegisterClassesDescendedFrom(typeof(BaseClass), ServiceLifetime.Scoped, RegisterClassAs.BaseOrClosedGenericClass, ClassRegex=@"^MyOtherNamespace\.")]
	//			internal class MyModule : RoslynjectModule
	//			{
	//			}

	//			public abstract class BaseClass {}
	//			public class Child1<T> : BaseClass {}
	//			public class Child2<T> : BaseClass {}
	//			public class Child1Child1 : Child1<int> {}
	//		}

	//		namespace MyOtherNamespace
	//		{
	//			using Microsoft.Extensions.DependencyInjection;
	//			using Morris.Roslynject;
	//			using MyNamespace;

	//			public class Child1Child1Child1 : Child1Child1 {}
	//		}
	//		""";

	//	const string expectedGeneratedCode =
	//		"""
	//		using Microsoft.Extensions.DependencyInjection;

	//		namespace MyNamespace
	//		{
	//			partial class MyModule
	//			{
	//				static partial void AfterRegister(IServiceCollection services);

	//				public static void Register(IServiceCollection services)
	//				{
	//					// RegisterClassesDescendedFrom(typeof(BaseClass), ServiceLifetime.Scoped, RegisterClassAs.BaseOrClosedGenericClass, ClassRegex=@"^MyOtherNamespace\.")
	//					services.AddScoped(typeof(global::MyNamespace.BaseClass), typeof(global::MyOtherNamespace.Child1Child1Child1));

	//					AfterRegister(services);
	//				}
	//			}
	//		}

	//		""";

	//	SourceGeneratorExecutor
	//		.AssertGeneratedCodeMatches(
	//			sourceCode: sourceCode,
	//			expectedGeneratedCode: expectedGeneratedCode
	//		);
	//}

}
