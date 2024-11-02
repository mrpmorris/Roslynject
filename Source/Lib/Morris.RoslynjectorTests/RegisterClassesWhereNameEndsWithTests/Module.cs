using Microsoft.Extensions.DependencyInjection;
using Morris.Roslynjector;

namespace Morris.RoslynjectorTests.RegisterClassesWhereNameEndsWithTests.ValidCandidatesAreRegistered;

[RegisterClassesWhereNameEndsWith(ServiceLifetime.Singleton, "Singleton")]
[RegisterClassesWhereNameEndsWith(ServiceLifetime.Scoped, "Scoped")]
[RegisterClassesWhereNameEndsWith(ServiceLifetime.Transient, "Transient")]
[RegisterClassesWhereNameEndsWith(ServiceLifetime.Singleton, "Invalid")]
public static partial class Module
{

}

public class Class1Singleton { }
public class Class2Singleton { }

public class Class1Scoped { }
public class Class2Scoped { }

public class Class1Transient { }
public class Class2Transient { }

public class OpenGenericInvalid<T> { }


