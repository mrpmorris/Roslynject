using System.Runtime.CompilerServices;

namespace Morris.Roslynject.StaticResources;

internal enum RegisterInterfaceAs
{
	ImplementedInterface,
	BaseInterface,
	BaseClosedGenericInterface
}

internal static class RegisterInterfaceAsParser
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static object? Parse(string value) =>
		value switch {
			nameof(RegisterInterfaceAs.ImplementedInterface) => RegisterInterfaceAs.ImplementedInterface,
			nameof(RegisterInterfaceAs.BaseInterface) => RegisterInterfaceAs.BaseInterface,
			nameof(RegisterInterfaceAs.BaseClosedGenericInterface) => RegisterInterfaceAs.BaseClosedGenericInterface,
			_ => null
		};
}

internal static partial class SourceCode
{
	public const string RegisterInterfaceAs =
	"""
		internal enum RegisterInterfaceAs
		{
			ImplementedInterface,
			BaseInterface,
			BaseOrClosedGenericInterface
		}
	""";
}