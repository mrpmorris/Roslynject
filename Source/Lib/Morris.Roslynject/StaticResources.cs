using Microsoft.Extensions.DependencyInjection;
#if NET9_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif

namespace Morris.Roslynject;

/// <summary>
/// Specifies which classes to register as the service key.
/// </summary>
internal enum RegisterClassAs
{
	/// <summary>
	/// Registers the class that was discovered as the key.
	/// </summary>
	DescendantClass,

	/// <summary>
	/// Registers the base class specified in the filter criteria as the key.
	/// </summary>
	BaseClass,

	/// <summary>
	/// Registers the base class as a closed-generic type as the key.
	/// </summary>
	BaseClosedGenericClass
}

/// <summary>
/// Specifies which interfaces to register as the service key.
/// </summary>
internal enum RegisterInterfaceAs
{
	/// <summary>
	/// Registers the interface declared on the class as the service key.
	/// </summary>
	ImplementedInterface,

	/// <summary>
	/// Registers the interface specified in the filter criteria as the key.
	/// </summary>
	BaseInterface,

	/// <summary>
	/// Registers the base interface as a closed-generic type as the key.
	/// </summary>
	BaseClosedGenericInterface
}

/// <summary>
/// Registers all classes descended from a specified base class.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
internal class RegisterClassesDescendedFromAttribute : Attribute
{
	/// <summary>
	/// Specifies which base type a class should be descended from
	/// to qualify for registration.
	/// </summary>
	public Type BaseClass { get; set; }

	/// <summary>
	/// Which lifetime to register the service with.
	/// </summary>
	public ServiceLifetime ServiceLifetime { get; set; }

	/// <summary>
	/// Specifies the key to use when registering the discovered class.
	/// </summary>
	public RegisterClassAs RegisterClassAs { get; set; }

	/// <summary>
	/// If not null, then only classes whose FullName matches the specified
	/// regex will be considered as candidates for registration.
	/// </summary>
#if NET9_0_OR_GREATER
	[StringSyntax(StringSyntaxAttribute.Regex)]
#endif
	public string? ClassRegex { get; set; }

	/// <summary>
	/// Creates an instance of the attribute.
	/// </summary>
	/// <param name="baseClass"><see cref="BaseClass"/></param>
	/// <param name="serviceLifetime"><see cref="ServiceLifetime"/></param>
	/// <param name="registerClassAs"><see cref="RegisterClassAs"/></param>
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

/// <summary>
/// Finds all classes that implement the specified marker-interface
/// (or a descendant of it) and registers them using the class
/// as the service key.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
internal class RegisterClassesWithMarkerInterfaceAttribute : Attribute
{
	/// <summary>
	/// Specifies which marker-interface (or descendant) a class
	/// should implement to be considered a candidate for registration.
	/// </summary>
	public Type BaseInterface { get; set; }

	/// <summary>
	/// Which lifetime to register the service with.
	/// </summary>
	public ServiceLifetime ServiceLifetime { get; set; }

	/// <summary>
	/// If not null, then only classes whose FullName matches the specified
	/// regex will be considered as candidates for registration.
	/// </summary>
#if NET9_0_OR_GREATER
	[StringSyntax(StringSyntaxAttribute.Regex)]
#endif
	public string? ClassRegex { get; set; }

	/// <summary>
	/// Creates an instance of the attribute.
	/// </summary>
	/// <param name="baseInterface"><see cref="BaseInterface"/></param>
	/// <param name="serviceLifetime"><see cref="ServiceLifetime"/></param>
	public RegisterClassesWithMarkerInterfaceAttribute(
		Type baseInterface,
		ServiceLifetime serviceLifetime)
	{
		BaseInterface = baseInterface;
		ServiceLifetime = serviceLifetime;
	}
}

/// <summary>
/// Finds all classes that implement the specified interface
/// (or descendant) and registers them using the interface
/// as the service key.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
internal class RegisterInterfacesOfTypeAttribute : Attribute
{
	/// <summary>
	/// Specifies which interface or descendant a class
	/// should implement to be considered a candidate for registration.
	/// </summary>
	public Type BaseInterface { get; set; }

	/// <summary>
	/// Which lifetime to register the service with.
	/// </summary>
	public ServiceLifetime ServiceLifetime { get; set; }

	/// <summary>
	/// Specifies the key to use when registering the class that
	/// implements the interface.
	/// </summary>
	public RegisterInterfaceAs RegisterInterfaceAs { get; set; }

	/// <summary>
	/// If not null, then only classes whose FullName matches the specified
	/// regex will be considered as candidates for registration.
	/// </summary>
#if NET9_0_OR_GREATER
	[StringSyntax(StringSyntaxAttribute.Regex)]
#endif
	public string? ClassRegex { get; set; }

	/// <summary>
	/// Creates an instance of the attribute.
	/// </summary>
	/// <param name="baseInterface"><see cref="BaseInterface"/></param>
	/// <param name="serviceLifetime"><see cref="ServiceLifetime"/></param>
	/// <param name="registerInterfaceAs"><see cref="RegisterInterfaceAs"/></param>
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

/// <summary>
/// Finds all classes that descend from the specified class
/// and registers them using their implemented interfaces
/// as the service key.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
internal class RegisterInterfacesOnClassesDescendedFromAttribute : Attribute
{
	/// <summary>
	/// Specifies which base class a class should descend from to
	/// be considered a candidate for registration.
	/// </summary>
	public Type BaseClass { get; set; }

	/// <summary>
	/// Which lifetime to register the service with.
	/// </summary>
	public ServiceLifetime ServiceLifetime { get; set; }

	/// <summary>
	/// If not null, then only classes whose FullName matches the specified
	/// regex will be considered as candidates for registration.
	/// </summary>
#if NET9_0_OR_GREATER
	[StringSyntax(StringSyntaxAttribute.Regex)]
#endif
	public string? ClassRegex { get; set; }

	/// <summary>
	/// Creates an instance of the attribute.
	/// </summary>
	/// <param name="baseClass"><see cref="BaseClass"/></param>
	/// <param name="serviceLifetime"><see cref="ServiceLifetime"/></param>
	public RegisterInterfacesOnClassesDescendedFromAttribute(
		Type baseClass,
		ServiceLifetime serviceLifetime)
	{
		BaseClass = baseClass;
		ServiceLifetime = serviceLifetime;
	}
}

/// <summary>
/// Marks a class as a module for Roslynject dependency registration.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
internal class RoslynjectModuleAttribute : Attribute
{
}
