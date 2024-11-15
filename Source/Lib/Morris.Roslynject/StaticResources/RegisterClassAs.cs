namespace Morris.Roslynject.StaticResources;

internal enum RegisterClassAs
{
	DescendantClass,
	BaseClass,
	BaseClosedGenericClass
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