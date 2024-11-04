using Microsoft.Extensions.DependencyInjection;
using Morris.Roslynject;

namespace Morris.RoslynjectTests.RegisterClassesWhereNameEndsWithTests.ValidCandidatesAreRegistered;

[RegisterClassesWhereNameEndsWith(ServiceLifetime.Singleton, "Singleton")]
[RegisterClassesWhereNameEndsWith(ServiceLifetime.Scoped, "Scoped")]
[RegisterClassesWhereNameEndsWith(ServiceLifetime.Transient, "Transient")]
[RegisterClassesWhereNameEndsWith(ServiceLifetime.Singleton, "Invalid")]
public partial class Module : RoslynjectModule
{

}

public class Class1Singleton { }
public class Class2Singleton { }

public class Class1Scoped { }
public class Class2Scoped { }

public class Class1Transient { }
public class Class2Transient { }

public class OpenGenericInvalid<T> { }


