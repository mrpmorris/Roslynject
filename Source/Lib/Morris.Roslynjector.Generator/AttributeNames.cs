namespace Morris.Roslynjector.Generator;

internal static class AttributeNames
{
    public static readonly string[] FullNames =
        [
            "Morris.Roslynjector.RegisterInterfacesAttribute",
            "Morris.Roslynjector.RegisterInterfacesWhereDescendsFromAttribute",
            "Morris.Roslynjector.RegisterInterfacesWhereNameEndsWithAttribute",
            "Morris.Roslynjector.RegisterClassesWhereDescendsFromAttribute",
            "Morris.Roslynjector.RegisterClassesWhereNameEndsWitAttribute"
        ];

    public static readonly string[] ShortNames =
        [
            "RegisterInterfaces",
            "RegisterInterfacesWhereDescendsFrom",
            "RegisterInterfacesWhereNameEndsWith",
            "RegisterClassesWhereDescendsFrom",
            "RegisterClassesWhereNameEndsWith"
        ];
}
