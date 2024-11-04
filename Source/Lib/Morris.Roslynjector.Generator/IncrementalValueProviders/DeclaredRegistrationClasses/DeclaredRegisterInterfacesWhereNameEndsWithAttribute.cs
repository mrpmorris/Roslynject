using Microsoft.CodeAnalysis;
using Morris.Roslynjector.Generator.Extensions;
using Morris.Roslynjector.Generator.IncrementalValueProviders.RegistrationClassOutputs;
using System.Collections.Immutable;

namespace Morris.Roslynjector.Generator.IncrementalValueProviders.DeclaredRegistrationClasses;

internal class DeclaredRegisterInterfacesWhereNameEndsWithAttribute : DeclaredRegisterAttributeBase, IEquatable<DeclaredRegisterInterfacesWhereNameEndsWithAttribute>
{
    public readonly string Suffix;
    private readonly Lazy<int> CachedHashCode;

    public DeclaredRegisterInterfacesWhereNameEndsWithAttribute(
        ServiceLifetime serviceLifetime,
        string suffix)
        : base(serviceLifetime)
    {
        Suffix = suffix;
        CachedHashCode = new Lazy<int>(() =>
            HashCode
            .Combine(
                ServiceLifetime,
                Suffix
            )
        );
    }

    public static bool operator ==(DeclaredRegisterInterfacesWhereNameEndsWithAttribute left, DeclaredRegisterInterfacesWhereNameEndsWithAttribute right) => left.Equals(right);
    public static bool operator !=(DeclaredRegisterInterfacesWhereNameEndsWithAttribute left, DeclaredRegisterInterfacesWhereNameEndsWithAttribute right) => !(left == right);

    public override IEnumerable<RegisterAttributeOutputBase> CreateOutput(ImmutableArray<INamedTypeSymbol> classesToRegister)
    {
        throw new NotImplementedException();
    }

    public override bool Equals(object obj) => obj is DeclaredRegisterInterfacesWhereNameEndsWithAttribute other && Equals(other);

    public bool Equals(DeclaredRegisterInterfacesWhereNameEndsWithAttribute other) =>
        ServiceLifetime == other.ServiceLifetime
        && Suffix == other.Suffix;

    public override int GetHashCode() => CachedHashCode.Value;

    public override bool Matches(INamedTypeSymbol typeSymbol) => false;

}
