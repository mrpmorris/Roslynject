using Microsoft.Extensions.DependencyInjection;

namespace Morris.Roslynjector;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class RegisterClassesWhereNameEndsWithAttribute : Attribute
{
    public ServiceLifetime ServiceLifetime { get; set; }
    public string Suffix { get; set; } = string.Empty;

    public RegisterClassesWhereNameEndsWithAttribute(
        ServiceLifetime serviceLifetime,
        string suffix)
    {
        Suffix = suffix ?? throw new ArgumentNullException(nameof(suffix));
        ServiceLifetime = serviceLifetime;
    }
}
