namespace Morris.Roslynject.StaticResources;

internal enum RegisterInterfaceAs
{
	ImplementedInterface,
	BaseInterface,
	BaseOrClosedGenericInterface
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