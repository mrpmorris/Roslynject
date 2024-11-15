// See https://aka.ms/new-console-template for more information
using Morris.Roslynject;

Console.WriteLine("Hello, World!");

public class MyModule : IRoslynjectModule
{

}

public class BaseClass { }
public class Child1 : BaseClass { }
public class Child2 : BaseClass { }
