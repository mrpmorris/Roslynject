using Microsoft.Extensions.DependencyInjection;

namespace Morris.Roslynjector;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class RegisterClassesWhereDescendsFromAttribute : Attribute
{
    public ServiceLifetime ServiceLifetime { get; set; }
    public Type BaseType { get; set; }

    public RegisterClassesWhereDescendsFromAttribute(
        ServiceLifetime serviceLifetime,
        Type baseType)
    {
        BaseType = baseType ?? throw new ArgumentNullException(nameof(baseType));
        ServiceLifetime = serviceLifetime;
    }
}
