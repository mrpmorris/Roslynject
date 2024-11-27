//using Microsoft.Extensions.DependencyInjection;
//using System.Diagnostics.CodeAnalysis;

//namespace Morris.Roslynject;

//[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
//internal class RegisterClassesWithMarkerInterfaceAttribute : Attribute
//{
//	public Type BaseInterface { get; set; }
//	public ServiceLifetime ServiceLifetime { get; set; }

//	#if NET9_0_OR_GREATER
//	[StringSyntax(StringSyntaxAttribute.Regex)]
//	#endif
//	public string? ClassRegex { get; set; }

//	public RegisterClassesWithMarkerInterfaceAttribute(
//		Type baseInterface,
//		ServiceLifetime serviceLifetime)
//	{
//		BaseInterface = baseInterface;
//		ServiceLifetime = serviceLifetime;
//	}
//}
