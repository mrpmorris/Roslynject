using Morris.Roslynject;

namespace Morris.RoslynjectTests.RegisterClassesDescendedFromAttributeTests.GenericSearchCriteriaTests;

[TestClass]
public class WhenRegisteringBaseClosedGenericClass
{
	[TestMethod]
	public void ThenRegistersBaseClosedGenericClassForEachDescendantClass()
	{
		const string sourceCode =
			"""
			using Microsoft.Extensions.DependencyInjection;
			using Morris.Roslynject;
			namespace MyNamespace;
			
			[RoslynjectModule]
			[RegisterClassesDescendedFrom(typeof(BaseClass<,>), ServiceLifetime.Scoped, RegisterClassAs.BaseClosedGenericClass)]
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
						// RegisterClassesDescendedFrom(typeof(BaseClass<,>), ServiceLifetime.Scoped, RegisterClassAs.BaseClosedGenericClass)
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

	[TestMethod]
	public void ThenRegistersBaseClassForEachClassMatchingRegex()
	{
		const string sourceCode =
			"""
			namespace MyNamespace
			{
				using Microsoft.Extensions.DependencyInjection;
				using Morris.Roslynject;

				[RoslynjectModule]
				[RegisterClassesDescendedFrom(typeof(BaseClass<,>), ServiceLifetime.Scoped, RegisterClassAs.BaseClosedGenericClass, ClassRegex=@"^MyOtherNamespace\.")]
				internal class MyModule : RoslynjectModule
				{
				}

				public class BaseClass<TRequest, TResponse> {}
				public class Child1 : BaseClass<int, bool> {}
				public class Child2 : BaseClass<string, bool> {}
				public class Child1Child1 : Child1 {}
			}

			namespace MyOtherNamespace
			{
				using Microsoft.Extensions.DependencyInjection;
				using Morris.Roslynject;
				using MyNamespace;

				public class Child1Child1Child1 : Child1Child1 {}
			}
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
						// RegisterClassesDescendedFrom(typeof(BaseClass<,>), ServiceLifetime.Scoped, RegisterClassAs.BaseClosedGenericClass, ClassRegex=@"^MyOtherNamespace\.")
						services.AddScoped(typeof(global::MyNamespace.BaseClass<int, bool>), typeof(global::MyOtherNamespace.Child1Child1Child1));

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

}
