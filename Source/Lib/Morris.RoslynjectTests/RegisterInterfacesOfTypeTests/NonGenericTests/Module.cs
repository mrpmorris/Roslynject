using Microsoft.Extensions.DependencyInjection;
using Morris.Roslynject;

namespace Morris.RoslynjectTests.RegisterInterfacesOfTypeTests.NonGenericTests;

[RegisterInterfacesOfType(ServiceLifetime.Singleton, typeof(ICommunicationStrategy))]
public static partial class Module
{

}

public interface ICommunicationStrategy { }
public class EmailStrategy : ICommunicationStrategy { }
public class SmsStrategy : ICommunicationStrategy { }

public interface IDescendantCommunicationStrategy : ICommunicationStrategy { }
public class DescendantCommunicationStrategy : IDescendantCommunicationStrategy { }
