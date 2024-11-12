using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Morris.Roslynject;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class RegisterInterfacesOfTypeAttribute : Attribute
{
	public Type BaseInterface { get; set; }
	public ServiceLifetime ServiceLifetime { get; set; }
	public RegisterInterfaceAs RegisterInterfaceAs { get; set; }

	[StringSyntax(StringSyntaxAttribute.Regex)]
	public string? InterfaceRegex { get; set; }

	public RegisterInterfacesOfTypeAttribute(
		Type baseInterface,
		ServiceLifetime serviceLifetime,
		RegisterInterfaceAs registerInterfaceAs)
	{
		BaseInterface = baseInterface;
		ServiceLifetime = serviceLifetime;
		RegisterInterfaceAs = registerInterfaceAs;
	}
}
