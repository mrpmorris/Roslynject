using Microsoft.Extensions.DependencyInjection;

namespace Morris.RoslynjectorTests.RegisterClassesWhereDescendsFromTests.GenericTests;

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
        Services.AssertIsRegistered(ServiceLifetime.Singleton, typeof(SingletonDescendant1OfGenericBase));
        Services.AssertIsRegistered(ServiceLifetime.Singleton, typeof(SingletonDescendant2OfGenericBase));
        Services.AssertIsRegistered(ServiceLifetime.Scoped, typeof(ScopedDescendant1OfGenericBase));
        Services.AssertIsRegistered(ServiceLifetime.Scoped, typeof(ScopedDescendant2OfGenericBase));
        Services.AssertIsRegistered(ServiceLifetime.Transient, typeof(TransientDescendant1OfGenericBase));
        Services.AssertIsRegistered(ServiceLifetime.Transient, typeof(TransientDescendant2OfGenericBase));
    }

    public RegisterClassesWhereDescendsFromTests()
    {
        Services = new ServiceCollection();
        Module.Register(Services);
    }


}
