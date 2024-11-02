using Microsoft.CodeAnalysis;
using Morris.Roslynjector.Generator.Extensions;
using System.Collections.Immutable;

namespace Morris.Roslynjector.Generator.IncrementalValueProviders.AttributeMetas;

internal class RegisterInterfacesWhereNameEndsWithAttributeMeta : RegisterAttributeMetaBase, IEquatable<RegisterInterfacesWhereNameEndsWithAttributeMeta>
{
    public readonly string Suffix;
    private readonly Lazy<int> CachedHashCode;

    public RegisterInterfacesWhereNameEndsWithAttributeMeta(
        ServiceLifetime serviceLifetime,
        string suffix,
        ImmutableArray<INamedTypeSymbol> classesToRegister)
        : base(serviceLifetime, classesToRegister)
    {
        Suffix = suffix;
        CachedHashCode = new Lazy<int>(() =>
            HashCode
            .Combine(
                ServiceLifetime,
                Suffix,
                ClassesToRegister.GetContentsHashCode(ClassSignatureComparer.Instance.GetHashCode)
            )
        );
    }

    public static bool operator ==(RegisterInterfacesWhereNameEndsWithAttributeMeta left, RegisterInterfacesWhereNameEndsWithAttributeMeta right) => left.Equals(right);
    public static bool operator !=(RegisterInterfacesWhereNameEndsWithAttributeMeta left, RegisterInterfacesWhereNameEndsWithAttributeMeta right) => !(left == right);
    public override bool Equals(object obj) => obj is RegisterInterfacesWhereNameEndsWithAttributeMeta other && Equals(other);

    public override RegisterAttributeMetaBase CloneWithClassesToRegister(
        ImmutableArray<INamedTypeSymbol> classes)
    =>
        new RegisterInterfacesWhereNameEndsWithAttributeMeta(
            serviceLifetime: ServiceLifetime,
            suffix: Suffix,
            classesToRegister: classes);

    public bool Equals(RegisterInterfacesWhereNameEndsWithAttributeMeta other) =>
        ServiceLifetime == other.ServiceLifetime
        && Suffix == other.Suffix
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
