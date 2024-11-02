using Microsoft.CodeAnalysis;
using Morris.Roslynjector.Generator.Extensions;
using System.Collections.Immutable;

namespace Morris.Roslynjector.Generator.IncrementalValueProviders.AttributeMetas;

internal class RegisterInterfaceAttributeMeta : RegisterAttributeMetaBase, IEquatable<RegisterInterfaceAttributeMeta>
{
    public readonly INamedTypeSymbol Interface;
    private readonly Lazy<int> CachedHashCode;

    public RegisterInterfaceAttributeMeta(
        ServiceLifetime serviceLifetime,
        INamedTypeSymbol @interface,
        ImmutableArray<INamedTypeSymbol> classesToRegister)
        : base(serviceLifetime, classesToRegister)
    {
        Interface = @interface;
        CachedHashCode = new Lazy<int>(() =>
            HashCode
            .Combine(
                ServiceLifetime,
                Interface.ToDisplayString(),
                ClassesToRegister.GetContentsHashCode(ClassSignatureComparer.Instance.GetHashCode)
            )
        );
    }

    public static bool operator ==(RegisterInterfaceAttributeMeta left, RegisterInterfaceAttributeMeta right) => left.Equals(right);
    public static bool operator !=(RegisterInterfaceAttributeMeta left, RegisterInterfaceAttributeMeta right) => !(left == right);
    public override bool Equals(object obj) => obj is RegisterInterfaceAttributeMeta other && Equals(other);

    public override RegisterAttributeMetaBase CloneWithClassesToRegister(
        ImmutableArray<INamedTypeSymbol> classes)
    =>
        new RegisterInterfaceAttributeMeta(
            serviceLifetime: ServiceLifetime,
            @interface: Interface,
            classesToRegister: classes);

    public bool Equals(RegisterInterfaceAttributeMeta other) =>
        ServiceLifetime == other.ServiceLifetime
        && ClassSignatureComparer.Instance.Equals(Interface, other.Interface)
        && Enumerable.SequenceEqual(
            ClassesToRegister,
            other.ClassesToRegister,
            ClassSignatureComparer.Instance);

    public override void GenerateCode(Action<string> writeLine)
    {
        string interfaceName = Interface.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        writeLine($"// RegisterClassesWhereNameEndsWith(ServiceLifetime.{ServiceLifetime}, typeof({interfaceName}))");
        foreach (INamedTypeSymbol type in ClassesToRegister)
        {
            string className = type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            writeLine($"services.Add{ServiceLifetime}(typeof({interfaceName}), typeof({className}));");
        }
    }

    public override int GetHashCode() => CachedHashCode.Value;

    public override bool Matches(INamedTypeSymbol typeSymbol) =>
        typeSymbol
        .Interfaces
        .Any(x => ClassIdentityComparer.Instance.Equals(Interface, x));
}
