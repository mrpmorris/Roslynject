using Microsoft.Extensions.DependencyInjection;
using Morris.Roslynjector;

namespace Morris.RoslynjectorTests.RegisterClassesWhereDescendsFromTests.NonGenericTests;

[RegisterClassesWhereDescendsFrom(ServiceLifetime.Singleton, typeof(SingletonBase))]
[RegisterClassesWhereDescendsFrom(ServiceLifetime.Scoped, typeof(ScopedBase))]
[RegisterClassesWhereDescendsFrom(ServiceLifetime.Transient, typeof(TransientBase))]
public static partial class Module
{

}

public abstract class SingletonBase { }
public class SingletonDescendant1 : SingletonBase { }
public class SingletonDescendant2 : SingletonBase { }
public class SingletonGrandchild : SingletonDescendant1 { }

public abstract class ScopedBase { }
public class ScopedDescendant1 : ScopedBase { }
public class ScopedDescendant2 : ScopedBase { }
public class ScopedGrandchild : ScopedDescendant1 { }

public abstract class TransientBase { }
public class TransientDescendant1 : TransientBase { }
public class TransientDescendant2 : TransientBase { }
public class TransientGrandchild : TransientDescendant1 { }
