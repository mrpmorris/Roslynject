using Microsoft.CodeAnalysis;
using Morris.Roslynjector.Generator.IncrementalValueProviders.RegistrationClassOutputs;
using System.Collections.Immutable;

namespace Morris.Roslynjector.Generator.IncrementalValueProviders.DeclaredRegistrationClasses;

internal class DeclaredRegisterClassesWhereNameEndsWithAttribute : DeclaredRegisterAttributeBase, IEquatable<DeclaredRegisterClassesWhereNameEndsWithAttribute>
{
    public readonly string Suffix;
    private readonly Lazy<int> CachedHashCode;

    public DeclaredRegisterClassesWhereNameEndsWithAttribute(
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

    public static bool operator ==(DeclaredRegisterClassesWhereNameEndsWithAttribute left, DeclaredRegisterClassesWhereNameEndsWithAttribute right) => left.Equals(right);
    public static bool operator !=(DeclaredRegisterClassesWhereNameEndsWithAttribute left, DeclaredRegisterClassesWhereNameEndsWithAttribute right) => !(left == right);

    public override IEnumerable<RegisterAttributeOutputBase> CreateOutput(ImmutableArray<INamedTypeSymbol> classesToRegister)
    {
        throw new NotImplementedException();
    }

    public override bool Equals(object obj) => obj is DeclaredRegisterClassesWhereNameEndsWithAttribute other && Equals(other);

    public bool Equals(DeclaredRegisterClassesWhereNameEndsWithAttribute other) =>
        ServiceLifetime == other.ServiceLifetime
        && Suffix == other.Suffix;

    public override int GetHashCode() => CachedHashCode.Value;

    public override bool Matches(INamedTypeSymbol typeSymbol) =>
        typeSymbol.Name.EndsWith(Suffix, StringComparison.OrdinalIgnoreCase);
}
