// See https://aka.ms/new-console-template for more information
using Morris.Roslynject;

Console.WriteLine("Hello, World!");

namespace Eggs
{
	[Roslynject(Find.DescendantsOf, typeof(BaseClass), Register.BaseType, WithLifetime.Scoped)]
	public partial class MyModule
	{
		public static void Hello() { }
	}

	public class BaseClass { }
	public class Child1 : BaseClass { }
	public class Child2 : BaseClass { }
	public class ThisShouldNotBeThere {
		public static void Hello() { }
	}
}