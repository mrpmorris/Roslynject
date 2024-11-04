using Microsoft.Extensions.DependencyInjection;
using Morris.RoslynjectTests.RegisterClassesWhereNameEndsWithTests.ValidCandidatesAreRegistered;

namespace Morris.RoslynjectTests.RegisterClassesWhereNameEndsWithTests;

[TestClass]
public class RegisterClassesWhereNameEndsWithTests
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
        Services.AssertIsRegistered(ServiceLifetime.Singleton, typeof(Class1Singleton));
        Services.AssertIsRegistered(ServiceLifetime.Singleton, typeof(Class2Singleton));

        Services.AssertIsRegistered(ServiceLifetime.Scoped, typeof(Class1Scoped));
        Services.AssertIsRegistered(ServiceLifetime.Scoped, typeof(Class2Scoped));

        Services.AssertIsRegistered(ServiceLifetime.Transient, typeof(Class1Transient));
        Services.AssertIsRegistered(ServiceLifetime.Transient, typeof(Class2Transient));
    }

    public RegisterClassesWhereNameEndsWithTests()
    {
        Services = new ServiceCollection();
        Module.Register(Services);
    }
}
