// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using Morris.Roslynject;

Console.WriteLine("Hello, World!");

namespace Eggs
{
	[RoslynjectModule]
	[Roslynject(ServiceLifetime.Scoped, Find.DescendantsOf, typeof(BaseClass), Register.BaseType)]
	public partial class MyModule
	{

	}

	public class BaseClass { }
	public class Child1 : BaseClass { }
	public class Child2 : BaseClass { }
}