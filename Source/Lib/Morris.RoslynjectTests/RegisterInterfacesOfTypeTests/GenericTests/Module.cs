using Microsoft.Extensions.DependencyInjection;
using Morris.Roslynject;

namespace Morris.RoslynjectTests.RegisterInterfacesOfTypeTests.GenericTests;

[RegisterInterfacesOfType(ServiceLifetime.Singleton, typeof(IGenericInterface<,>))]
public partial class Module : RoslynjectModule
{

}

public interface IGenericInterface<T1, T2> { }
public class FirstValidClass : IGenericInterface<int, string> { }
public class SecondValidClass : IGenericInterface<Guid, object?> { }

public interface IDescendantGenericInterface<T1, T2> : IGenericInterface<T1, T2> { }
public class DescendantClass : IDescendantGenericInterface<int, string> { }
