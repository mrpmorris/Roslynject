using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Morris.Roslynject;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class RegisterClassesWithMarkerInterfaceAttribute : Attribute
{
	public required Type BaseInterface { get; set; }
	public required ServiceLifetime ServiceLifetime { get; set; }

	[StringSyntax(StringSyntaxAttribute.Regex)]
	public string? ClassRegex { get; set; }

	public RegisterClassesWithMarkerInterfaceAttribute(
		Type baseInterface,
		ServiceLifetime serviceLifetime)
	{
		BaseInterface = baseInterface ?? throw new ArgumentNullException(nameof(baseInterface));
		ServiceLifetime = serviceLifetime;
	}
}
