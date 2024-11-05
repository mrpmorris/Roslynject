using Microsoft.Extensions.DependencyInjection;
using Morris.RoslynjectTests.RegisterClassesWhereDescendsFromTests.NonGenericTests;

namespace Morris.RoslynjectTests.RegisterClassesOfTypeTests.NonGenericTests;

[TestClass]
public class RegisterClassesOfTypeTests
{
    private readonly IServiceCollection Services;

    [TestMethod]
    public void OnlyValidCandidatesAreRegistered()
    {
        Assert.AreEqual(9, Services.Count);
    }

    [TestMethod]
    public void ClassesAreRegisteredWithCorrectScope()
    {
        Services.AssertIsRegistered(ServiceLifetime.Singleton, typeof(SingletonDescendant1));
        Services.AssertIsRegistered(ServiceLifetime.Singleton, typeof(SingletonDescendant2));
        Services.AssertIsRegistered(ServiceLifetime.Singleton, typeof(SingletonGrandchild));

        Services.AssertIsRegistered(ServiceLifetime.Scoped, typeof(ScopedDescendant1));
        Services.AssertIsRegistered(ServiceLifetime.Scoped, typeof(ScopedDescendant2));
        Services.AssertIsRegistered(ServiceLifetime.Scoped, typeof(ScopedGrandchild));

        Services.AssertIsRegistered(ServiceLifetime.Transient, typeof(TransientDescendant1));
        Services.AssertIsRegistered(ServiceLifetime.Transient, typeof(TransientDescendant2));
        Services.AssertIsRegistered(ServiceLifetime.Transient, typeof(TransientGrandchild));
    }

    public RegisterClassesOfTypeTests()
    {
        Services = new ServiceCollection();
        Module.Register(Services);
    }


}
