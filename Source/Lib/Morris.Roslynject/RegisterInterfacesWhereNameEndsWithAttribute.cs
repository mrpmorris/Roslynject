using Microsoft.Extensions.DependencyInjection;

namespace Morris.Roslynject;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class RegisterInterfacesWhereNameEndsWithAttribute : Attribute
{
    public ServiceLifetime ServiceLifetime { get; set; }
    public string Suffix { get; set; } = string.Empty;

    public RegisterInterfacesWhereNameEndsWithAttribute(
        ServiceLifetime serviceLifetime,
        string suffix)
    {
        Suffix = suffix ?? throw new ArgumentNullException(nameof(suffix));
        ServiceLifetime = serviceLifetime;
    }
}
