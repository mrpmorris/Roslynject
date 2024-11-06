using Microsoft.Extensions.DependencyInjection;
using Morris.Roslynject;

namespace Morris.RoslynjectTests.RegisterInterfacesDescendedFromTests.NonGenericTests;

[RegisterInterfacesDescendedFrom(ServiceLifetime.Singleton, typeof(ICommunicationStrategy))]
public partial class Module : RoslynjectModule
{

}

public interface ICommunicationStrategy { }
public class EmailStrategy : ICommunicationStrategy { }
public class SmsStrategy : ICommunicationStrategy { }

public interface IDescendantCommunicationStrategy : ICommunicationStrategy { }
public class DescendantCommunicationStrategy : IDescendantCommunicationStrategy { }
