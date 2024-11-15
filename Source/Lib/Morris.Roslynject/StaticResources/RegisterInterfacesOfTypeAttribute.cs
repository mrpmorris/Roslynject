//using Microsoft.Extensions.DependencyInjection;
//using System.Diagnostics.CodeAnalysis;

//namespace Morris.Roslynject;

//[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
//internal class RegisterInterfacesOfTypeAttribute : Attribute
//{
//	public Type BaseInterface { get; set; }
//	public ServiceLifetime ServiceLifetime { get; set; }
//	public RegisterInterfaceAs RegisterInterfaceAs { get; set; }

//	#if NET9_0_OR_GREATER
//	[StringSyntax(StringSyntaxAttribute.Regex)]
//	#endif
//	public string? InterfaceRegex { get; set; }

//	public RegisterInterfacesOfTypeAttribute(
//		Type baseInterface,
//		ServiceLifetime serviceLifetime,
//		RegisterInterfaceAs registerInterfaceAs)
//	{
//		BaseInterface = baseInterface;
//		ServiceLifetime = serviceLifetime;
//		RegisterInterfaceAs = registerInterfaceAs;
//	}
//}
