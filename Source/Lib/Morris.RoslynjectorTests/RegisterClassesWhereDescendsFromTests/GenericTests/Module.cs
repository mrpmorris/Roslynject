using Microsoft.Extensions.DependencyInjection;
using Morris.Roslynjector;

namespace Morris.RoslynjectorTests.RegisterClassesWhereDescendsFromTests.GenericTests;

[RegisterClassesWhereDescendsFrom(ServiceLifetime.Singleton, typeof(GenericForSingletonBase<>))]
[RegisterClassesWhereDescendsFrom(ServiceLifetime.Scoped, typeof(GenericForScopedBase<>))]
[RegisterClassesWhereDescendsFrom(ServiceLifetime.Transient, typeof(GenericForTransientBase<>))]
public static partial class Module
{

}

public abstract class GenericForSingletonBase<T> { }
public class SingletonDescendant1OfGenericBase : GenericForSingletonBase<int> { }
public class SingletonDescendant2OfGenericBase : GenericForSingletonBase<string> { }
public class GenericSingletonDescendantOfGenericBase<T> : GenericForSingletonBase<T> { }

public abstract class GenericForScopedBase<T> { }
public class ScopedDescendant1OfGenericBase : GenericForScopedBase<int> { }
public class ScopedDescendant2OfGenericBase : GenericForScopedBase<string> { }
public class GenericScopedDescendantOfGenericBase<T> : GenericForScopedBase<T> { }

public abstract class GenericForTransientBase<T> { }
public class TransientDescendant1OfGenericBase : GenericForTransientBase<int> { }
public class TransientDescendant2OfGenericBase : GenericForTransientBase<string> { }
public class GenericTransientDescendantOfGenericBase<T> : GenericForTransientBase<T> { }
