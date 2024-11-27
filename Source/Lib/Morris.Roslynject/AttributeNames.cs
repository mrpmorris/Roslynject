using Morris.Roslynject.StaticResources;
using System.Runtime.CompilerServices;

namespace Morris.Roslynject;

internal static class AttributeNames
{
	public static readonly string[] ShortNames =
	[
		RemoveAttributeFromName(nameof(SourceCode.RegisterClassesDescendedFromAttribute))
		//"RegisterClassesDescendedFrom",
		//"RegisterClassesWithMarkerInterface",
		//"RegisterInterfacesOfType",
		//"RegisterInterfacesOnClassesDescendedFrom"
	];

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static string RemoveAttributeFromName(string name) =>
		name.Substring(0, name.Length - 9);
}
