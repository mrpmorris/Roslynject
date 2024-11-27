using System.Runtime.CompilerServices;

namespace Morris.Roslynject.StaticResources;

internal enum RegisterClassAs
{
	DescendantClass,
	BaseClass,
	BaseClosedGenericClass
}

internal static class RegisterClassAsParser
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static object? Parse(string value) =>
		value switch {
			nameof(RegisterClassAs.DescendantClass) => RegisterClassAs.DescendantClass,
			nameof(RegisterClassAs.BaseClass) => RegisterClassAs.BaseClass,
			nameof(RegisterClassAs.BaseClosedGenericClass) => RegisterClassAs.BaseClosedGenericClass,
			_ => null
		};
}

internal static partial class SourceCode
{
	public const string RegisterClassAs =
	"""
		internal enum RegisterClassAs
		{
			DescendantClass,
			BaseClass,
			BaseClosedGenericClass
		}
	""";
}