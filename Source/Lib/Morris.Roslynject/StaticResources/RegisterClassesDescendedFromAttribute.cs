using System.Collections.Immutable;

namespace Morris.Roslynject.StaticResources;

internal static partial class SourceCode
{
	public const string RegisterClassesDescendedFromAttribute =
	"""
		[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
		internal class RegisterClassesDescendedFromAttribute : Attribute
		{
			public Type BaseClass { get; set; }
			public ServiceLifetime ServiceLifetime { get; set; }
			public RegisterClassAs RegisterClassAs { get; set; }

			#if NET9_0_OR_GREATER
			[StringSyntax(StringSyntaxAttribute.Regex)]
			#endif
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
	""";
}

