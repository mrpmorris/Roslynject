//using Microsoft.Extensions.DependencyInjection;

//namespace Morris.RoslynjectTests.RegisterInterfaceTests.NonGenericTests;

//[TestClass]
//public class RegisterInterfaceTests
//{
//    private readonly IServiceCollection Services;

//    [TestMethod]
//    public void OnlyValidCandidatesAreRegistered()
//    {
//        Assert.AreEqual(2, Services.Count);
//    }

//    [TestMethod]
//    public void InterfacesAreRegisteredWithCorrectClasses()
//    {
//        Services.AssertIsRegistered(
//            lifetime: ServiceLifetime.Singleton,
//            serviceType: typeof(ICommunicationStrategy),
//            implementationType: typeof(EmailStrategy)
//        );

//        Services.AssertIsRegistered(
//            lifetime: ServiceLifetime.Singleton,
//            serviceType: typeof(ICommunicationStrategy),
//            implementationType: typeof(SmsStrategy)
//        );
//    }

//    public RegisterInterfaceTests()
//    {
//        Services = new ServiceCollection();
//        //Module.Register(Services);
//    }


//}
