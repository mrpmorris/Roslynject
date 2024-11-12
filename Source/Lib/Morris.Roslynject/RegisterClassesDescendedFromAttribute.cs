using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Morris.Roslynject;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class RegisterClassesDescendedFromAttribute : Attribute
{
	public Type BaseClass { get; set; }
	public ServiceLifetime ServiceLifetime { get; set; }
	public RegisterClassAs RegisterClassAs { get; set; }

	[StringSyntax(StringSyntaxAttribute.Regex)]
	public string? ClassRegex { get; set; }

	public RegisterClassesDescendedFromAttribute(
		Type baseClass,
		ServiceLifetime serviceLifetime,
		RegisterClassAs registerClassAs)
	{
		BaseClass = baseClass;
		ServiceLifetime = serviceLifetime;
		RegisterClassAs = registerClassAs;
	}
}
