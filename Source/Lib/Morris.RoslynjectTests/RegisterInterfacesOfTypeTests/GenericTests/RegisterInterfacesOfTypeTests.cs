using Microsoft.Extensions.DependencyInjection;

namespace Morris.RoslynjectTests.RegisterInterfacesOfTypeTests.GenericTests;

[TestClass]
public class RegisterInterfacesOfTypeTests
{
    private readonly IServiceCollection Services;

    [TestMethod]
    public void OnlyValidCandidatesAreRegistered()
    {
        Assert.AreEqual(2, Services.Count);
    }

    [TestMethod]
    public void InterfacesAreRegisteredWithCorrectClasses()
    {
        Services.AssertIsRegistered(
            lifetime: ServiceLifetime.Singleton,
            serviceType: typeof(IGenericInterface<int, string>),
            implementationType: typeof(FirstValidClass)
        );

        Services.AssertIsRegistered(
            lifetime: ServiceLifetime.Singleton,
            serviceType: typeof(IGenericInterface<Guid, object?>),
            implementationType: typeof(SecondValidClass)
        );
    }

    public RegisterInterfacesOfTypeTests()
    {
        Services = new ServiceCollection();
        Module.Register(Services);
    }
}
