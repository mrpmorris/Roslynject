using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Morris.Roslynject;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class RegisterInterfacesOfTypeAttribute : Attribute
{
	public Type BaseInterface { get; set; }
	public ServiceLifetime ServiceLifetime { get; set; }
	public InterfaceRegistration As { get; set; }

#if NET8_0_OR_GREATER
	[StringSyntax(StringSyntaxAttribute.Regex)]
#endif
	public string? InterfaceRegex { get; set; }

	public RegisterInterfacesOfTypeAttribute(
		Type baseInterface,
		ServiceLifetime serviceLifetime,
		InterfaceRegistration @as)
	{
		BaseInterface = baseInterface;
		ServiceLifetime = serviceLifetime;
		As = @as;
	}
}
