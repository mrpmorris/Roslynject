using Microsoft.Extensions.DependencyInjection;

namespace Morris.RoslynjectTests.RegisterInterfacesDescendedFromTests.NonGenericTests;

[TestClass]
public class RegisterInterfacesDescendedFromTests
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
            serviceType: typeof(ICommunicationStrategy),
            implementationType: typeof(EmailStrategy)
        );

        Services.AssertIsRegistered(
            lifetime: ServiceLifetime.Singleton,
            serviceType: typeof(ICommunicationStrategy),
            implementationType: typeof(SmsStrategy)
        );
    }

    public RegisterInterfacesDescendedFromTests()
    {
        Services = new ServiceCollection();
        Module.Register(Services);
    }
}
