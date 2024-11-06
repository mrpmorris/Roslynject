using Microsoft.Extensions.DependencyInjection;

namespace Morris.RoslynjectTests.RegisterClassesDescendedFromTests.NonGenericTests;

[TestClass]
public class RegisterClassesDescendedFromTests
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

    public RegisterClassesDescendedFromTests()
    {
        Services = new ServiceCollection();
        Module.Register(Services);
    }
}
