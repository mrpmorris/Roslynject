using Microsoft.CodeAnalysis;
using Morris.Roslynjector.Generator.Extensions;
using System.Collections.Immutable;

namespace Morris.Roslynjector.Generator.IncrementalValueProviders.AttributeMetas;

internal class RegisterInterfacesAttributeMeta : RegisterAttributeMetaBase, IEquatable<RegisterInterfacesAttributeMeta>
{
    public readonly INamedTypeSymbol Interface;
    private readonly Lazy<int> CachedHashCode;

    public RegisterInterfacesAttributeMeta(
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

    public static bool operator ==(RegisterInterfacesAttributeMeta left, RegisterInterfacesAttributeMeta right) => left.Equals(right);
    public static bool operator !=(RegisterInterfacesAttributeMeta left, RegisterInterfacesAttributeMeta right) => !(left == right);
    public override bool Equals(object obj) => obj is RegisterInterfacesAttributeMeta other && Equals(other);

    public override RegisterAttributeMetaBase CloneWithClassesToRegister(
        ImmutableArray<INamedTypeSymbol> classes)
    =>
        new RegisterInterfacesAttributeMeta(
            serviceLifetime: ServiceLifetime,
            @interface: Interface,
            classesToRegister: classes);

    public bool Equals(RegisterInterfacesAttributeMeta other) =>
        ServiceLifetime == other.ServiceLifetime
        && ClassSignatureComparer.Instance.Equals(Interface, other.Interface)
        && Enumerable.SequenceEqual(
            ClassesToRegister,
            other.ClassesToRegister,
            ClassSignatureComparer.Instance);

    public override void GenerateCode(Action<string> writeLine)
    {
    }

    public override int GetHashCode() => CachedHashCode.Value;

    public override bool Matches(INamedTypeSymbol typeSymbol) => false;
}
