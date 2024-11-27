// See https://aka.ms/new-console-template for more information
using Morris.Roslynject;

Console.WriteLine("Hello, World!");

namespace Eggs
{
	[RoslynjectModule]
	[RegisterClassesDescendedFrom(typeof(BaseClass), Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped, Morris.Roslynject.RegisterClassAs.BaseClass)]
	public partial class MyModule
	{

	}

	public class BaseClass { }
	public class Child1 : BaseClass { }
	public class Child2 : BaseClass { }
}