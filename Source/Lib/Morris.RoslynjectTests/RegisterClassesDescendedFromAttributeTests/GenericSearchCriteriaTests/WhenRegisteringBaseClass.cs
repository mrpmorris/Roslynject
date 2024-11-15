namespace Morris.RoslynjectTests.RegisterClassesDescendedFromAttributeTests.GenericSearchCriteriaTests;

[TestClass]
public class WhenRegisteringBaseClass
{
	[TestMethod]
	public void ThenRegistersBaseClassForEachClosedGenericDescendantClass()
	{
		const string sourceCode =
			"""
			using Microsoft.Extensions.DependencyInjection;
			using Morris.Roslynject;
			using System.Collections.Generic;
			namespace MyNamespace;
			
			[RegisterClassesDescendedFrom(typeof(List<>), ServiceLifetime.Scoped, RegisterClassAs.BaseClass)]
			internal class MyModule : RoslynjectModule
			{
			}
			
			public class Child1 : List<int> {}
			public class Child2 : List<string> {}
			public class Child2Child1 : Child2 {}
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
						// RegisterClassesDescendedFrom(typeof(List<>), ServiceLifetime.Scoped, RegisterClassAs.BaseClass)
						services.AddScoped(typeof(global::System.Collections.Generic.List<>), typeof(global::MyNamespace.Child1));
						services.AddScoped(typeof(global::System.Collections.Generic.List<>), typeof(global::MyNamespace.Child2));
						services.AddScoped(typeof(global::System.Collections.Generic.List<>), typeof(global::MyNamespace.Child2Child1));

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
				using System.Collections.Generic;
				
				[RegisterClassesDescendedFrom(typeof(List<>), ServiceLifetime.Scoped, RegisterClassAs.BaseClass, ClassRegex=@"^MyOtherNamespace\.")]
				internal class MyModule : RoslynjectModule
				{
				}
			
				public class Child1<T> : List<T> {}
				public class Child2<T> : List<T> {}
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
						// RegisterClassesDescendedFrom(typeof(List<>), ServiceLifetime.Scoped, RegisterClassAs.BaseClass, ClassRegex=@"^MyOtherNamespace\.")
						services.AddScoped(typeof(global::System.Collections.Generic.List<>), typeof(global::MyOtherNamespace.Child1Child1Child1));

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
			using System.Collections.Generic;
			namespace MyNamespace;

			[RegisterClassesDescendedFrom(typeof(List<>), ServiceLifetime.Scoped, RegisterClassAs.BaseClass)]
			internal class MyModule : RoslynjectModule
			{
			}

			public abstract class Child1 : List<int> {}
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
			using System.Collections.Generic;
			namespace MyNamespace;

			[RegisterClassesDescendedFrom(typeof(List<>), ServiceLifetime.Scoped, RegisterClassAs.BaseClass)]
			internal class MyModule : RoslynjectModule
			{
			}

			public class Child1<T> : List<T> {}
			""";

		SourceGeneratorExecutor.AssertGeneratedEmptyModule(sourceCode);
	}
}
