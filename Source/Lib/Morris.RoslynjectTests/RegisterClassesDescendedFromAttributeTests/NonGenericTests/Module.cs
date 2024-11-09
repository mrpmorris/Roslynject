using Microsoft.Extensions.DependencyInjection;
using Morris.Roslynject;

namespace Morris.RoslynjectTests.RegisterClassesDescendedFromAttributeTests.NonGenericTests;

[RegisterClassesDescendedFrom(typeof(CommunicationStrategy), ServiceLifetime.Singleton, ClassRegistration.BaseClass, ClassRegex = @"^Morris\.RoslynjectTests\.RegisterClassesDescendedFromAttributeTests\.NonGenericTests\.")]
internal partial class Module : RoslynjectModule
{
}

public abstract class CommunicationStrategy { }
public class EmailStrategy : CommunicationStrategy { }
public abstract class TelephoneStrategy : CommunicationStrategy { }
public class AutomatedCallStrategy : TelephoneStrategy { }
public class SmsStrategy : TelephoneStrategy { }
