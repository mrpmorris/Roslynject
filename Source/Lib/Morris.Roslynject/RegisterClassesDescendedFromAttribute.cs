using Microsoft.Extensions.DependencyInjection;

namespace Morris.Roslynject;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class RegisterClassesDescendedFromAttribute : Attribute
{
    public Type BaseClass { get; set; }
    public ServiceLifetime ServiceLifetime { get; set; }
    public ClassAs As {  get; set; }
    public string? ClassRegex { get; set; }

    public RegisterClassesDescendedFromAttribute(
        Type baseType,
        ServiceLifetime serviceLifetime,
        ClassAs @as)
    {
        BaseClass = baseType ?? throw new ArgumentNullException(nameof(baseType));
        ServiceLifetime = serviceLifetime;
        As = @as;
    }
}
