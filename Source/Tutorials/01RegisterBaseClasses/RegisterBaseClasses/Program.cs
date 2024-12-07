// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using Morris.Roslynject;

Console.WriteLine("Hello, World!");

namespace Eggs
{
	[RoslynjectModule(ClassRegex = "Eggs")]
	[Roslynject(Find.DescendantsOf, typeof(BaseClass), Register.BaseType, WithLifetime.Scoped)]
	public partial class MyModule
	{
		public static void Hello() { }
	}

	public class BaseClass { }
	public class Child1 : BaseClass { }
	public class Child2 : BaseClass { }
}