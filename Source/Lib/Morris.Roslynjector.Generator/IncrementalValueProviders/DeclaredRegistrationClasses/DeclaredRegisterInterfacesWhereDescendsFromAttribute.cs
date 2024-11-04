using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Morris.Roslynjector.Generator.IncrementalValueProviders.RegistrationClassOutputs;
using System.Collections.Immutable;

namespace Morris.Roslynjector.Generator.IncrementalValueProviders.DeclaredRegistrationClasses;

internal class DeclaredRegisterInterfacesWhereDescendsFromAttribute : DeclaredRegisterAttributeBase, IEquatable<DeclaredRegisterInterfacesWhereDescendsFromAttribute>
{
    public readonly INamedTypeSymbol BaseInterfaceType;
    private readonly Lazy<int> CachedHashCode;

    public DeclaredRegisterInterfacesWhereDescendsFromAttribute(
        AttributeSyntax attributeSyntax,
        ServiceLifetime serviceLifetime,
        INamedTypeSymbol baseInterfaceType)
    : base(
            attributeSyntax: attributeSyntax,
            serviceLifetime: serviceLifetime)
    {
        BaseInterfaceType = baseInterfaceType;
        CachedHashCode = new Lazy<int>(() =>
            HashCode
            .Combine(
                ServiceLifetime,
                BaseInterfaceType.ToDisplayString()
             )
        );
    }

    public static bool operator ==(DeclaredRegisterInterfacesWhereDescendsFromAttribute left, DeclaredRegisterInterfacesWhereDescendsFromAttribute right) => left.Equals(right);
    public static bool operator !=(DeclaredRegisterInterfacesWhereDescendsFromAttribute left, DeclaredRegisterInterfacesWhereDescendsFromAttribute right) => !(left == right);

    public override RegisterAttributeOutputBase? CreateOutput(ImmutableArray<INamedTypeSymbol> injectionCandidates)
    {
        throw new NotImplementedException();
    }

    public override bool Equals(object obj) => obj is DeclaredRegisterInterfacesWhereDescendsFromAttribute other && Equals(other);

    public bool Equals(DeclaredRegisterInterfacesWhereDescendsFromAttribute other) =>
        ServiceLifetime == other.ServiceLifetime
        && TypeIdentifyWithInheritanceComparer.Default.Equals(BaseInterfaceType, other.BaseInterfaceType);

    public override int GetHashCode() => CachedHashCode.Value;

    public override bool Matches(INamedTypeSymbol typeSymbol) => false;

}
