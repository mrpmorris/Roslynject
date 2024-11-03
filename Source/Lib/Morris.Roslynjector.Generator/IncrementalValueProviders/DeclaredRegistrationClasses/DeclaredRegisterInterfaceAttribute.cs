using Microsoft.CodeAnalysis;
using Morris.Roslynjector.Generator.Extensions;
using System.Collections.Immutable;

namespace Morris.Roslynjector.Generator.IncrementalValueProviders.DeclaredRegistrationClasses;

internal class DeclaredRegisterInterfaceAttribute : DeclaredRegisterAttributeBase, IEquatable<DeclaredRegisterInterfaceAttribute>
{
    public readonly INamedTypeSymbol InterfaceType;
    private readonly Lazy<int> CachedHashCode;

    public DeclaredRegisterInterfaceAttribute(
        ServiceLifetime serviceLifetime,
        INamedTypeSymbol interfaceType)
        : base(serviceLifetime)
    {
        InterfaceType = interfaceType;
        CachedHashCode = new Lazy<int>(() =>
            HashCode
            .Combine(
                ServiceLifetime,
                InterfaceType.ToDisplayString()
            )
        );
    }

    public static bool operator ==(DeclaredRegisterInterfaceAttribute left, DeclaredRegisterInterfaceAttribute right) => left.Equals(right);
    public static bool operator !=(DeclaredRegisterInterfaceAttribute left, DeclaredRegisterInterfaceAttribute right) => !(left == right);
    public override bool Equals(object obj) => obj is DeclaredRegisterInterfaceAttribute other && Equals(other);

    public bool Equals(DeclaredRegisterInterfaceAttribute other) =>
        ServiceLifetime == other.ServiceLifetime
        && ClassSignatureComparer.Instance.Equals(InterfaceType, other.InterfaceType));

    public override int GetHashCode() => CachedHashCode.Value;

    public override bool Matches(INamedTypeSymbol typeSymbol) =>
        typeSymbol
        .Interfaces
        .Any(x =>
            ClassIdentityComparer.Instance.Equals(
                InterfaceType.ConstructedFrom,
                x.ConstructedFrom
            )
         );
}
