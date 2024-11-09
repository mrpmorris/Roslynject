using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Morris.Roslynject;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class RegisterInterfacesOnClassesDescendedFromAttribute : Attribute
{
	public Type BaseClass { get; set; }
	public ServiceLifetime ServiceLifetime { get; set; }

	[StringSyntax(StringSyntaxAttribute.Regex)]
	public string? InterfaceRegex { get; set; }

	public RegisterInterfacesOnClassesDescendedFromAttribute(
		Type baseClass,
		ServiceLifetime serviceLifetime)
	{
		BaseClass = baseClass ?? throw new ArgumentNullException(nameof(baseClass));
		ServiceLifetime = serviceLifetime;
	}
}
