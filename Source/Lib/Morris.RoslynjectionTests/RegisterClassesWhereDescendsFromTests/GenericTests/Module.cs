using Microsoft.Extensions.DependencyInjection;
using Morris.Roslynject;

namespace Morris.RoslynjectTests.RegisterClassesWhereDescendsFromTests.GenericTests;

[RegisterClassesWhereDescendsFrom(ServiceLifetime.Singleton, typeof(SingletonBase<>))]
[RegisterClassesWhereDescendsFrom(ServiceLifetime.Scoped, typeof(ScopedBase<>))]
[RegisterClassesWhereDescendsFrom(ServiceLifetime.Transient, typeof(TransientBase<>))]
[RegisterClassesWhereDescendsFrom(ServiceLifetime.Scoped, typeof(GenericBase<>))]
public partial class Module : RoslynjectModule
{

}

public abstract class SingletonBase<T> { }
public class SingletonDescendant1 : SingletonBase<int> { }
public class SingletonDescendant2 : SingletonBase<string> { }
public class GenericDescendantOfSingletonBase<T> : SingletonBase<T> { }

public abstract class ScopedBase<T> { }
public class ScopedDescendant1 : ScopedBase<int> { }
public class ScopedDescendant2 : ScopedBase<string> { }
public class GenericDescendantOfScopedBase<T> : ScopedBase<T> { }

public abstract class TransientBase<T> { }
public class TransientDescendant1 : TransientBase<int> { }
public class TransientDescendant2 : TransientBase<string> { }
public class GenericDescendantOfTransientBase<T> : TransientBase<T> { }

public class GenericBase<T> { }
public class GenericChild<T> : GenericBase<T> { }
public class GrandchildOfGenericBase : GenericChild<int> { }

