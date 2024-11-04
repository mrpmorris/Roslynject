using Microsoft.CodeAnalysis;
using Morris.Roslynjector.Generator.Extensions;
using Morris.Roslynjector.Generator.IncrementalValueProviders.RegistrationClassOutputs;
using System.Collections.Immutable;

namespace Morris.Roslynjector.Generator.IncrementalValueProviders.DeclaredRegistrationClasses;

internal class DeclaredRegisterClassesWhereDescendsFromAttribute : DeclaredRegisterAttributeBase, IEquatable<DeclaredRegisterClassesWhereDescendsFromAttribute>
{
    public readonly INamedTypeSymbol BaseClassType;
    private readonly Lazy<int> CachedHashCode;

    public DeclaredRegisterClassesWhereDescendsFromAttribute(
        ServiceLifetime serviceLifetime,
        INamedTypeSymbol baseClassType)
        : base(serviceLifetime)
    {
        BaseClassType = baseClassType;
        CachedHashCode = new Lazy<int>(() =>
            HashCode
            .Combine(
                ServiceLifetime,
                BaseClassType.ToDisplayString()
             )
        );
    }

    public static bool operator ==(DeclaredRegisterClassesWhereDescendsFromAttribute left, DeclaredRegisterClassesWhereDescendsFromAttribute right) => left.Equals(right);
    public static bool operator !=(DeclaredRegisterClassesWhereDescendsFromAttribute left, DeclaredRegisterClassesWhereDescendsFromAttribute right) => !(left == right);

    public override RegisterAttributeOutputBase? CreateOutput(ImmutableArray<INamedTypeSymbol> injectionCandidates) =>
        new RegisterClassesWhereDescendsFromAttributeOutput(
            serviceLifetime: ServiceLifetime,
            baseClassType: BaseClassType,
            injectionCandidates: injectionCandidates);

    public override bool Equals(object obj) => obj is DeclaredRegisterClassesWhereDescendsFromAttribute other && Equals(other);

    public bool Equals(DeclaredRegisterClassesWhereDescendsFromAttribute other) =>
        ServiceLifetime == other.ServiceLifetime
        && TypeIdentifyWithInheritanceComparer.Default.Equals(BaseClassType, other.BaseClassType);

    public override int GetHashCode() => CachedHashCode.Value;

    public override bool Matches(INamedTypeSymbol typeSymbol) =>
        typeSymbol.DescendsFrom(BaseClassType);
}
