//using Microsoft.Extensions.DependencyInjection;

//namespace Morris.RoslynjectorTests.RegisterInterfaceTests.GenericTests;

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
//            serviceType: typeof(IGenericInterface<int, string>),
//            implementationType: typeof(FirstValidClass)
//        );

//        Services.AssertIsRegistered(
//            lifetime: ServiceLifetime.Singleton,
//            serviceType: typeof(IGenericInterface<Guid, object?>),
//            implementationType: typeof(SecondValidClass)
//        );
//    }

//    public RegisterInterfaceTests()
//    {
//        Services = new ServiceCollection();
//        //Module.Register(Services);
//    }


//}
