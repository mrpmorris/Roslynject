Filters

public enum InterfaceAs
{
    ImplementedInterface,
    BaseInterface,
    BaseOrClosedGenericInterface
}

public enum InterfaceOptions
{
	SharedClassInstance
}

public enum ClassAs
{
    DescendantClass,
    BaseClass,
    BaseOrClosedGenericClass
}

RegisterClassesDescendedFrom(
	Type baseClass,
	ServiceLifetime serviceLifetime,
	ClassAs as,
	string classRegex);

RegisterClassesWithMarkerInterface(
	Type baseInterface,
	ServiceLifetime serviceLifetime,
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

