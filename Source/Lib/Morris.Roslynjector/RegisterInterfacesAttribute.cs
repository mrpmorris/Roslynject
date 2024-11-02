using Microsoft.Extensions.DependencyInjection;

namespace Morris.Roslynjector;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class RegisterInterfacesAttribute : Attribute
{
    public ServiceLifetime ServiceLifetime { get; set; }
    public Type Class { get; set; } = null!;

    public RegisterInterfacesAttribute(
        ServiceLifetime serviceLifetime,
        Type @class)
    {
        Class = @class ?? throw new ArgumentNullException(nameof(@class));
        ServiceLifetime = serviceLifetime;
    }
}
