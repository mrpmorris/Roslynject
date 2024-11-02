using Microsoft.Extensions.DependencyInjection;
using Morris.Roslynjector;

namespace Morris.RoslynjectorTests.RegisterClassesWhereNameEndsWithTests.ValidCandidatesAreRegistered;

[RegisterClassesWhereNameEndsWith(ServiceLifetime.Singleton, "SingletonClass")]
[RegisterClassesWhereNameEndsWith(ServiceLifetime.Scoped, "ScopedClass")]
[RegisterClassesWhereNameEndsWith(ServiceLifetime.Transient, "TransientClass")]
[RegisterClassesWhereNameEndsWith(ServiceLifetime.Singleton, "InvalidClass")]
public static partial class Module
{

}

public class Class1SingletonClass { }
public class Class2SingletonClass { }

public class Class1ScopedClass { }
public class Class2ScopedClass { }

public class Class1TransientClass { }
public class Class2TransientClass { }

public class OpenGenericInvalidClass<T> { }


