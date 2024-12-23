namespace Morris.RoslynjectTests;

[TestClass]
public class FindClassDescendantsAndRegisterDiscoveredInterfaces
{
	[TestMethod]
	public void WhenCandidateIsConcrete_ThenRegisters()
	{
		string sourceCode =
			$$"""
			using Morris.Roslynject;
			namespace Tests
			{
				public class BaseClass {}
				public interface IInterface1 {}
				public interface IInterface2 {}
				public class Child1 : BaseClass, IInterface1 {}
				public class Child2 : BaseClass, IInterface1, IInterface2 {}

				[Roslynject(Find.DescendantsOf, typeof(BaseClass), Register.DiscoveredInterfaces, WithLifetime.Scoped)]
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
						//Find DescendantsOf, Type Tests.BaseClass, Register DiscoveredInterfaces, WithLifetime Scoped
						services.AddScoped(typeof(global::Tests.IInterface1), typeof(global::Tests.Child1));
						services.AddScoped(typeof(global::Tests.IInterface1), typeof(global::Tests.Child2));
						services.AddScoped(typeof(global::Tests.IInterface2), typeof(global::Tests.Child2));
			
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
