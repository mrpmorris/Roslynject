using Microsoft.Extensions.DependencyInjection;
using Morris.Roslynjector;

namespace Morris.RoslynjectorTests.RegisterClassesWhereDescendsFromTests.NonGenericTests;

[RegisterClassesWhereDescendsFrom(ServiceLifetime.Singleton, typeof(NonGenericForSingletonBase))]
[RegisterClassesWhereDescendsFrom(ServiceLifetime.Scoped, typeof(NonGenericForScopedBase))]
[RegisterClassesWhereDescendsFrom(ServiceLifetime.Transient, typeof(NonGenericForTransientBase))]
public static partial class Module
{

}

public abstract class NonGenericForSingletonBase { }
public class SingletonDescendant1OfNonGenericBase : NonGenericForSingletonBase { }
public class SingletonDescendant2OfNonGenericBase : NonGenericForSingletonBase { }

public abstract class NonGenericForScopedBase { }
public class ScopedDescendant1OfNonGenericBase : NonGenericForScopedBase { }
public class ScopedDescendant2OfNonGenericBase : NonGenericForScopedBase { }

public abstract class NonGenericForTransientBase { }
public class TransientDescendant1OfNonGenericBase : NonGenericForTransientBase { }
public class TransientDescendant2OfNonGenericBase : NonGenericForTransientBase { }

