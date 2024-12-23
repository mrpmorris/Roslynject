namespace Morris.RoslynjectTests;

[TestClass]
public class FindClassDescendantsAndRegisterBaseClosedGenericType
{
	[TestMethod]
	public void WhenCandidateIsConcrete_ThenRegisters()
	{
		string sourceCode =
			$$"""
			using Morris.Roslynject;
			namespace Tests
			{
				public class BaseClass<T> { }
				public class Child1<T> : BaseClass<T> { }
				public class Child1GrandChild1 : Child1<int> { }

				[Roslynject(Find.DescendantsOf, typeof(BaseClass<>), Register.BaseClosedGenericType, WithLifetime.Scoped)]
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
						//Find DescendantsOf, Type Tests.BaseClass<>, Register BaseClosedGenericType, WithLifetime Scoped
						services.AddScoped(typeof(global::Tests.BaseClass<int>), typeof(global::Tests.Child1GrandChild1));

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
	public void WhenBaseTypeIsNotGeneric_ThenRegistersNonGenericBaseType()
	{
		string sourceCode =
			$$"""
			using Morris.Roslynject;
			namespace Tests
			{
				public class BaseClass { }
				public class Child1<T> : BaseClass { }
				public class Child1GrandChild1 : Child1<int> { }

				[Roslynject(Find.DescendantsOf, typeof(BaseClass), Register.BaseClosedGenericType, WithLifetime.Scoped)]
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
						//Find DescendantsOf, Type Tests.BaseClass, Register BaseClosedGenericType, WithLifetime Scoped
						services.AddScoped(typeof(global::Tests.BaseClass), typeof(global::Tests.Child1GrandChild1));

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
				public class BaseClass<T> { }
				public class Child1<T> : BaseClass<T> { }
				public abstract class Child1GrandChild1 : Child1<int> { }
			
				[Roslynject(Find.DescendantsOf, typeof(BaseClass<>), Register.BaseClosedGenericType, WithLifetime.Scoped)]
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
						//Find DescendantsOf, Type Tests.BaseClass<>, Register BaseClosedGenericType, WithLifetime Scoped

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
