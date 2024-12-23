namespace Morris.RoslynjectTests;

[TestClass]
public class FindClassDescendantsAndRegisterBaseType
{
	[TestMethod]
	public void WhenCandidateIsConcrete_ThenRegisters()
	{
		string sourceCode =
			$$"""
			using Morris.Roslynject;
			namespace Tests
			{
				public class BaseClass { }
				public class Child1 : BaseClass { }
				public class Child2 : BaseClass { }

				[Roslynject(Find.DescendantsOf, typeof(BaseClass), Register.BaseType, WithLifetime.Scoped)]
				public partial class MyModule {}
			}
			""";

		string expectedGeneratedCode =
			$$"""
			using Microsoft.Extensions.DependencyInjection;
			namespace Tests
			{
				partial class MyModule
				{
					static partial void AfterRegisterServices(IServiceCollection services);

					public static void RegisterServices(IServiceCollection services)
					{
						//Find DescendantsOf, Type Tests.BaseClass, Register BaseType, WithLifetime Scoped
						services.AddScoped(typeof(global::Tests.BaseClass), typeof(global::Tests.Child1));
						services.AddScoped(typeof(global::Tests.BaseClass), typeof(global::Tests.Child2));

						AfterRegisterServices(services);
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
	public void WhenCandidateIsAbstract_ThenDoesNotRegister()
	{
		string sourceCode =
			$$"""
			using Morris.Roslynject;
			namespace Tests
			{
				public class BaseClass { }
				public abstract class Child1 : BaseClass { }
				public class Child2 : BaseClass { }

				[Roslynject(Find.DescendantsOf, typeof(BaseClass), Register.BaseType, WithLifetime.Scoped)]
				public partial class MyModule {}
			}
			""";

		string expectedGeneratedCode =
			$$"""
			using Microsoft.Extensions.DependencyInjection;
			namespace Tests
			{
				partial class MyModule
				{
					static partial void AfterRegisterServices(IServiceCollection services);

					public static void RegisterServices(IServiceCollection services)
					{
						//Find DescendantsOf, Type Tests.BaseClass, Register BaseType, WithLifetime Scoped
						services.AddScoped(typeof(global::Tests.BaseClass), typeof(global::Tests.Child2));

						AfterRegisterServices(services);
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
	public void WhenCandidateIsOpenGeneric_ThenDoesNotRegister()
	{
		string sourceCode =
			$$"""
			using Morris.Roslynject;
			namespace Tests
			{
				public class BaseClass { }
				public class Child1<T> : BaseClass { }
				public class Child2 : BaseClass { }

				[Roslynject(Find.DescendantsOf, typeof(BaseClass), Register.BaseType, WithLifetime.Scoped)]
				public partial class MyModule {}
			}
			""";

		string expectedGeneratedCode =
			$$"""
			using Microsoft.Extensions.DependencyInjection;
			namespace Tests
			{
				partial class MyModule
				{
					static partial void AfterRegisterServices(IServiceCollection services);

					public static void RegisterServices(IServiceCollection services)
					{
						//Find DescendantsOf, Type Tests.BaseClass, Register BaseType, WithLifetime Scoped
						services.AddScoped(typeof(global::Tests.BaseClass), typeof(global::Tests.Child2));

						AfterRegisterServices(services);
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
