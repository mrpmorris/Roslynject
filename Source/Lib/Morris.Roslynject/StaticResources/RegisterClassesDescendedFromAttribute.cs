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
		""";
}

