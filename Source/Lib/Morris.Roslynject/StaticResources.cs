#if NET9_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif

namespace Morris.Roslynject
{
	/// <summary>
	/// Scans the assembly and registers all dependencies that match
	/// the given criteria.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	internal class RoslynjectAttribute : Attribute
	{
		/// <summary>
		/// If not null, then only depdendency classes with a full name matching
		/// this regular expression will be registered.
		/// </summary>
#if NET9_0_OR_GREATER
	[StringSyntax(StringSyntaxAttribute.Regex)]
#endif
		public string? ClassRegex { get; set; }

		/// <summary>
		/// The criteria to use when scanning for candidates to register.
		/// </summary>
		public Find Find { get; set; }

		/// <summary>
		/// Specifies what should be used as the service key when
		/// registering the dependency.
		/// </summary>
		public Register Register { get; set; }

		/// <summary>
		/// If not null, then only service key types with a full name matching
		/// this regular expression will be registered.
		/// </summary>
#if NET9_0_OR_GREATER
	[StringSyntax(StringSyntaxAttribute.Regex)]
#endif
		public string? ServiceKeyRegex { get; set; }

		/// <summary>
		/// The type to use when scanning for candidates to register.
		/// </summary>
		public Type Type { get; set; }

		/// <summary>
		/// The lifetime to use when registering the dependency.
		/// </summary>
		public WithLifetime WithLifetime { get; set; }

		public RoslynjectAttribute(
			Find find,
			Type type,
			Register register,
			WithLifetime withLifetime)
		{
			Find = find;
			Type = type;
			Register = register;
			WithLifetime = withLifetime;
		}
	}

	/// <summary>
	/// Specifies the search criteria.
	/// </summary>
	internal enum Find
	{
		/// <summary>
		/// Only considers descendants of the specified type
		/// as candidates for registration.
		/// </summary>
		DescendantsOf,
		/// <summary>
		/// Considers descendants of the specified type
		/// and the specified type itself as candidates for registration.
		/// </summary>
		AnyTypeOf,
		/// <summary>
		/// Only considers the exact specified type when determining
		/// candidates for registration.
		/// </summary>
		Exactly
	}

	/// <summary>
	/// Specifies what should be used as the service key.
	/// </summary>
	internal enum Register
	{
		/// <summary>
		/// The service key will be the type specified in the filter criteria.
		/// </summary>
		BaseType,
		/// <summary>
		/// The service key will be the base type as a closed-generic type.
		/// </summary>
		BaseClosedGenericType,
		/// <summary>
		/// The service key will be the class discovered.
		/// </summary>
		DiscoveredClasses,
		/// <summary>
		/// A service key will registered for each interface implemented
		/// by the class discovered.
		/// </summary>
		DiscoveredInterfaces
	}

	/// <summary>
	/// Specifies the lifetime of a service.
	/// </summary>
	public enum WithLifetime
	{
		/// <summary>
		/// Specifies that a single instance of the service will be created.
		/// </summary>
		Singleton,
		/// <summary>
		/// Specifies that a new instance of the service will be created for each scope.
		/// </summary>
		/// <remarks>
		/// In ASP.NET Core applications a scope is created around each server request.
		/// </remarks>
		Scoped,
		/// <summary>
		/// Specifies that a new instance of the service will be created every time it is requested.
		/// </summary>
		Transient
	}
}