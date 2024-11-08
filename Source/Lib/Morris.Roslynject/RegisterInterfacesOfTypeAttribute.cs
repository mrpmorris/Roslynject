using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Morris.Roslynject;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class RegisterInterfacesOfTypeAttribute : Attribute
{
	public required Type BaseInterface { get; set; }
	public required ServiceLifetime ServiceLifetime { get; set; }
	public required InterfaceAs As { get; set; }

	[StringSyntax(StringSyntaxAttribute.Regex)]
	public string? InterfaceRegex { get; set; }

	public RegisterInterfacesOfTypeAttribute(
		Type baseInterface,
		ServiceLifetime serviceLifetime,
		InterfaceAs @as)
	{
		BaseInterface = baseInterface ?? throw new ArgumentNullException(nameof(baseInterface));
		ServiceLifetime = serviceLifetime;
		As = @as;
	}
}
