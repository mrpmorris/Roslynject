namespace Morris.RoslynjectTests.RegisterClassesDescendedFromAttributeTests.NonGenericSearchCriteriaTests;

[TestClass]
public class WhenRegisteringDescendantClasses
{
	[TestMethod]
	public void ThenRegistersAllConcreteDescendantClasses()
	{
		const string sourceCode =
			"""
			using Microsoft.Extensions.DependencyInjection;
			using Morris.Roslynject;
			namespace MyNamespace;
			
			[RoslynjectModule]
			[RegisterClassesDescendedFrom(typeof(BaseClass), ServiceLifetime.Scoped, RegisterClassAs.DescendantClass)]
			internal class MyModule : RoslynjectModule
			{
			}
			
			public class BaseClass {}
			public class Child1 : BaseClass {}
			public class Child2 : BaseClass {}
			public class Child1Child1 : Child1 {}
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
						// RegisterClassesDescendedFrom(typeof(BaseClass), ServiceLifetime.Scoped, RegisterClassAs.DescendantClass)
						services.AddScoped(typeof(global::MyNamespace.Child1));
						services.AddScoped(typeof(global::MyNamespace.Child2));
						services.AddScoped(typeof(global::MyNamespace.Child1Child1));

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
	public void ThenRegistersClosedGenericClasses()
	{
		const string sourceCode =
			"""
			using Microsoft.Extensions.DependencyInjection;
			using Morris.Roslynject;
			namespace MyNamespace;
			
			[RoslynjectModule]
			[RegisterClassesDescendedFrom(typeof(BaseClass), ServiceLifetime.Scoped, RegisterClassAs.DescendantClass)]
			internal class MyModule : RoslynjectModule
			{
			}
			
			public class BaseClass {}
			public class Child1<T> : BaseClass {}
			public class Child2<T> : BaseClass {}
			public class Child1Child1 : Child1<int> {}
			public class Child1Child1Child1 : Child1Child1 {}
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
						// RegisterClassesDescendedFrom(typeof(BaseClass), ServiceLifetime.Scoped, RegisterClassAs.DescendantClass)
						services.AddScoped(typeof(global::MyNamespace.Child1Child1));
						services.AddScoped(typeof(global::MyNamespace.Child1Child1Child1));

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
	public void ThenRegistersOnlyClassesMatchingRegex()
	{
		const string sourceCode =
			"""
			namespace MyNamespace
			{
				using Microsoft.Extensions.DependencyInjection;
				using Morris.Roslynject;
				
				[RoslynjectModule]
				[RegisterClassesDescendedFrom(typeof(BaseClass), ServiceLifetime.Scoped, RegisterClassAs.DescendantClass, ClassRegex=@"^MyOtherNamespace\.")]
				internal class MyModule : RoslynjectModule
				{
				}
			
				public class BaseClass {}
				public class Child1<T> : BaseClass {}
				public class Child2<T> : BaseClass {}
				public class Child1Child1 : Child1<int> {}
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
						// RegisterClassesDescendedFrom(typeof(BaseClass), ServiceLifetime.Scoped, RegisterClassAs.DescendantClass, ClassRegex=@"^MyOtherNamespace\.")
						services.AddScoped(typeof(global::MyOtherNamespace.Child1Child1Child1));

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
	public void ThenDoesNotRegisterAbstractClasses()
	{
		const string sourceCode =
			"""
			using Microsoft.Extensions.DependencyInjection;
			using Morris.Roslynject;
			namespace MyNamespace;

			[RoslynjectModule]
			[RegisterClassesDescendedFrom(typeof(BaseClass), ServiceLifetime.Scoped, RegisterClassAs.DescendantClass)]
			internal class MyModule : RoslynjectModule
			{
			}

			public class BaseClass {}
			public abstract class Child1 : BaseClass {}
			""";

		SourceGeneratorExecutor.AssertGeneratedEmptyModule(sourceCode);
	}


	[TestMethod]
	public void ThenDoesNotRegisterOpenGenericClasses()
	{
		const string sourceCode =
			"""
			using Microsoft.Extensions.DependencyInjection;
			using Morris.Roslynject;
			namespace MyNamespace;

			[RoslynjectModule]
			[RegisterClassesDescendedFrom(typeof(BaseClass), ServiceLifetime.Scoped, RegisterClassAs.DescendantClass)]
			internal class MyModule : RoslynjectModule
			{
			}

			public class BaseClass {}
			public class Child1<T> : BaseClass {}
			""";

		SourceGeneratorExecutor.AssertGeneratedEmptyModule(sourceCode);
	}
}
