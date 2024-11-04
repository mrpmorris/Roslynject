//using Microsoft.Extensions.DependencyInjection;
//using Morris.Roslynject;

//namespace Morris.RoslynjectTests.RegisterInterfaceTests.GenericTests;

//[RegisterInterface(ServiceLifetime.Singleton, typeof(IGenericInterface<,>))]
//public static partial class Module
//{

//}

//public interface IGenericInterface<T1, T2> { }
//public class FirstValidClass : IGenericInterface<int, string> { }
//public class SecondValidClass : IGenericInterface<Guid, object?> { }

//public interface IDescendantGenericInterface<T1, T2> : IGenericInterface<T1, T2> { }
//public class InvalidClass : IDescendantGenericInterface<int, string> { }
