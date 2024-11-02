using Microsoft.Extensions.DependencyInjection;

namespace Morris.RoslynjectorTests.RegisterClassesWhereDescendsFromTests.NonGenericTests;

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
        Services.AssertIsRegistered(ServiceLifetime.Singleton, typeof(SingletonDescendant1OfNonGenericBase));
        Services.AssertIsRegistered(ServiceLifetime.Singleton, typeof(SingletonDescendant2OfNonGenericBase));
        Services.AssertIsRegistered(ServiceLifetime.Scoped, typeof(ScopedDescendant1OfNonGenericBase));
        Services.AssertIsRegistered(ServiceLifetime.Scoped, typeof(ScopedDescendant2OfNonGenericBase));
        Services.AssertIsRegistered(ServiceLifetime.Transient, typeof(TransientDescendant1OfNonGenericBase));
        Services.AssertIsRegistered(ServiceLifetime.Transient, typeof(TransientDescendant2OfNonGenericBase));
    }

    public RegisterClassesWhereDescendsFromTests()
    {
        Services = new ServiceCollection();
        Module.Register(Services);
    }


}
