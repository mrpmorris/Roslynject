using Microsoft.Extensions.DependencyInjection;

namespace Morris.Roslynject;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class RegisterInterfaceAttribute : Attribute
{
    public ServiceLifetime ServiceLifetime { get; set; }
    public Type Class { get; set; } = null!;

    public RegisterInterfaceAttribute(
        ServiceLifetime serviceLifetime,
        Type @class)
    {
        Class = @class ?? throw new ArgumentNullException(nameof(@class));
        ServiceLifetime = serviceLifetime;
    }
}
