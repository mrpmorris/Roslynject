﻿using Microsoft.Extensions.DependencyInjection;
using Morris.Roslynject;

namespace Morris.RoslynjectTests.RegisterClassesOfTypeTests.NonGenericTests;

[RegisterClassesOfType(ServiceLifetime.Singleton, typeof(SingletonBase))]
[RegisterClassesOfType(ServiceLifetime.Scoped, typeof(ScopedBase))]
[RegisterClassesOfType(ServiceLifetime.Transient, typeof(TransientBase))]
public partial class Module : RoslynjectModule
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