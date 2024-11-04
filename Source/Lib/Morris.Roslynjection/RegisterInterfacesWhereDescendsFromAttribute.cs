using Microsoft.Extensions.DependencyInjection;

namespace Morris.Roslynject;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class RegisterInterfacesWhereDescendsFromAttribute : Attribute
{
    public ServiceLifetime ServiceLifetime { get; set; }
    public Type BaseType { get; set; }

    public RegisterInterfacesWhereDescendsFromAttribute(
        ServiceLifetime serviceLifetime,
        Type baseType)
    {
        BaseType = baseType ?? throw new ArgumentNullException(nameof(baseType));
        ServiceLifetime = serviceLifetime;
    }
}