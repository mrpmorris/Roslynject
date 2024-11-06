using Microsoft.Extensions.DependencyInjection;

namespace Morris.Roslynject;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class RegisterClassesDescendedFromAttribute : Attribute
{
    public ServiceLifetime ServiceLifetime { get; set; }
    public Type BaseType { get; set; }

    public RegisterClassesDescendedFromAttribute(
        ServiceLifetime serviceLifetime,
        Type baseType)
    {
        BaseType = baseType ?? throw new ArgumentNullException(nameof(baseType));
        ServiceLifetime = serviceLifetime;
    }
}
