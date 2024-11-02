using Microsoft.Extensions.DependencyInjection;
using Morris.RoslynjectorTests.RegisterClassesWhereNameEndsWithTests.ValidCandidatesAreRegistered;

namespace Morris.RoslynjectorTests.RegisterClassesWhereNameEndsWithTests;

[TestClass]
public class RegisterClassesWhereDescendsFromTests
{
    private readonly IServiceCollection Services;

    [TestMethod]
    public void OnlyValidCandidatesAreRegistered()
    {
        Assert.AreEqual(6, Services.Count);
    }

    [TestMethod]
    public void ClassesAreRegisteredWithCorrectScope()
    {
        Services.AssertIsRegistered(ServiceLifetime.Singleton, typeof(Class1SingletonClass));
        Services.AssertIsRegistered(ServiceLifetime.Singleton, typeof(Class2SingletonClass));
        Services.AssertIsRegistered(ServiceLifetime.Scoped, typeof(Class1ScopedClass));
        Services.AssertIsRegistered(ServiceLifetime.Scoped, typeof(Class2ScopedClass));
        Services.AssertIsRegistered(ServiceLifetime.Transient, typeof(Class1TransientClass));
        Services.AssertIsRegistered(ServiceLifetime.Transient, typeof(Class2TransientClass));
    }

    public RegisterClassesWhereDescendsFromTests()
    {
        Services = new ServiceCollection();
        Module.Register(Services);
    }


}
