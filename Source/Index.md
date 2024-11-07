Filters

public enum InterfaceAs
{
	BaseInterface
	ImplementedInterface
	OpenGenericOrBaseInterface
}

public enum InterfaceOptions
{
	SharedClassInstance
}

public enum ClassAs
{
	BaseClass,
	DescendantClass,
	OpenGenericOrBaseClass
}

RegisterClassesDescendedFrom(
	Type baseClass,
	ServiceLifetime serviceLifetime,
	ClassAs as,
	string classRegex);

RegisterInterfacesOfType(
	Type baseInterface,
	ServiceLifetime serviceLifetime,
	InterfaceAs as,
	string interfaceRegex);

RegisterInterfacesOnClassesDescendedFrom(
	Type baseClass,
	ServiceLifetime serviceLifetime,
	string interfaceRegex);

RegisterClassesWithMarkerInterfaceOfType(
	Type baseInterface,
	ServiceLifetime serviceLifetime,
	string classRegex);