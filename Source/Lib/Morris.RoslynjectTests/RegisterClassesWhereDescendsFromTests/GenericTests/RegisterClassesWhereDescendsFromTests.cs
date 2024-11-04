using Microsoft.Extensions.DependencyInjection;

namespace Morris.RoslynjectTests.RegisterClassesWhereDescendsFromTests.GenericTests;

[TestClass]
public class RegisterClassesWhereDescendsFromTests
{
    private readonly IServiceCollection Services;

    [TestMethod]
    public void OnlyValidCandidatesAreRegistered()
    {
        Assert.AreEqual(7, Services.Count);
    }

    [TestMethod]
    public void ClassesAreRegisteredWithCorrectScope()
    {
        Services.AssertIsRegistered(ServiceLifetime.Singleton, typeof(SingletonDescendant1));
        Services.AssertIsRegistered(ServiceLifetime.Singleton, typeof(SingletonDescendant2));

        Services.AssertIsRegistered(ServiceLifetime.Scoped, typeof(ScopedDescendant1));
        Services.AssertIsRegistered(ServiceLifetime.Scoped, typeof(ScopedDescendant2));

        Services.AssertIsRegistered(ServiceLifetime.Transient, typeof(TransientDescendant1));
        Services.AssertIsRegistered(ServiceLifetime.Transient, typeof(TransientDescendant2));

        Services.AssertIsRegistered(ServiceLifetime.Scoped, typeof(GrandchildOfGenericBase));
    }

    public RegisterClassesWhereDescendsFromTests()
    {
        Services = new ServiceCollection();
        //Module.Register(Services);
    }


}
