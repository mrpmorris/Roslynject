using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Morris.Roslynject;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class RegisterClassesDescendedFromAttribute : Attribute
{
	public Type BaseClass { get; set; }
	public ServiceLifetime ServiceLifetime { get; set; }
	public ClassRegistration RegisterAs { get; set; }

#if NET8_0_OR_GREATER
	[StringSyntax(StringSyntaxAttribute.Regex)]
#endif
	public string? ClassRegex { get; set; }

	public RegisterClassesDescendedFromAttribute(
		Type baseClass,
		ServiceLifetime serviceLifetime,
		ClassRegistration registerAs)
	{
		BaseClass = baseClass;
		ServiceLifetime = serviceLifetime;
		RegisterAs = registerAs;
	}
}
