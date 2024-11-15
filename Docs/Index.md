Filters

public enum RegisterInterfaceAs
{
    ImplementedInterface,
    BaseInterface,
    BaseOrClosedGenericInterface
}

public enum RegisterInterfaceOptions
{
	SharedClassInstance
}

public enum RegisterClassAs
{
    DescendantClass,
    BaseClass,
    BaseClosedGenericClass
}

RegisterClassesDescendedFrom(
	Type baseClass,
	ServiceLifetime serviceLifetime,
	RegisterClassAs registerClassAs,
	string classRegex);

RegisterClassesWithMarkerInterface(
	Type baseInterface,
	ServiceLifetime serviceLifetime,
	string classRegex);


RegisterInterfacesOfType(
	Type baseInterface,
	ServiceLifetime serviceLifetime,
	RegisterInterfaceAs registerInterfaceAs,
	string interfaceRegex);

RegisterInterfacesOnClassesDescendedFrom(
	Type baseClass,
	ServiceLifetime serviceLifetime,
	string interfaceRegex);

