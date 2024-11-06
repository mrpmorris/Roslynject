using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Morris.Roslynject.Generator.IncrementalValueProviders.RegistrationClassOutputs;
using System.Collections.Immutable;

namespace Morris.Roslynject.Generator.IncrementalValueProviders.DeclaredRegistrationClasses;

internal class DeclaredRegisterInterfacesDescendedFromAttribute : DeclaredRegisterAttributeBase, IEquatable<DeclaredRegisterInterfacesDescendedFromAttribute>
{
    public readonly INamedTypeSymbol BaseInterfaceType;
    private readonly Lazy<int> CachedHashCode;

    public DeclaredRegisterInterfacesDescendedFromAttribute(
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
                base.GetHashCode(),
                BaseInterfaceType.ToDisplayString()
             )
        );
    }

    public static bool operator ==(
        DeclaredRegisterInterfacesDescendedFromAttribute left,
        DeclaredRegisterInterfacesDescendedFromAttribute right)
    =>
        left.Equals(right);

    public static bool operator !=(
        DeclaredRegisterInterfacesDescendedFromAttribute left,
        DeclaredRegisterInterfacesDescendedFromAttribute right)
    =>
        !(left == right);

    public override RegisterAttributeOutputBase? CreateOutput(ImmutableArray<INamedTypeSymbol> injectionCandidates)
    =>
        RegisterInterfacesDescendedFromAttributeOutput.Create(
            attributeSourceCode: AttributeSourceCode,
            serviceLifetime: ServiceLifetime,
            baseInterfaceType: BaseInterfaceType,
            injectionCandidates: injectionCandidates);

    public override bool Equals(object obj) =>
        obj is DeclaredRegisterInterfacesDescendedFromAttribute other
        && Equals(other);

    public bool Equals(DeclaredRegisterInterfacesDescendedFromAttribute other) =>
        base.Equals(other)
        && TypeIdentifyWithInheritanceComparer.Default.Equals(BaseInterfaceType, other.BaseInterfaceType);

    public override int GetHashCode() => CachedHashCode.Value;

    public override bool Matches(INamedTypeSymbol typeSymbol) => false;
}
