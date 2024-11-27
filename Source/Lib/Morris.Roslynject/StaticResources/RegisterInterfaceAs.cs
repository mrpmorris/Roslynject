using System.Runtime.CompilerServices;

namespace Morris.Roslynject.StaticResources;

internal enum RegisterInterfaceAs
{
	ImplementedInterface,
	BaseInterface,
	BaseClosedGenericInterface
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