Filters

public enum As
{
	BaseInterface
	ImplementedInterface
	OpenGenericOrBaseInterface
}

RegisterClassesDescendedFrom(
	Type baseClass,
	ServiceLifetime serviceLifetime,
	As as,
	string classRegex);

RegisterInterfacesOfType(
	Type baseInterface,
	ServiceLifetime serviceLifetime,
	As as,
	string interfaceRegex);

RegisterInterfacesOnClassesDescendedFrom(
	Type baseClass,
	ServiceLifetime serviceLifetime,
	string interfaceRegex);

RegisterClassesWithMarkerInterfaceOfType(
	Type baseInterface,
	ServiceLifetime serviceLifetime,
	string classRegex);