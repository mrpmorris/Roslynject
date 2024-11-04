using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Morris.Roslynject.Generator.IncrementalValueProviders.RegistrationClassOutputs;
using System.Collections.Immutable;

namespace Morris.Roslynject.Generator.IncrementalValueProviders.DeclaredRegistrationClasses;

internal class DeclaredRegisterInterfacesWhereNameEndsWithAttribute : DeclaredRegisterAttributeBase, IEquatable<DeclaredRegisterInterfacesWhereNameEndsWithAttribute>
{
    public readonly string Suffix;
    private readonly Lazy<int> CachedHashCode;

    public DeclaredRegisterInterfacesWhereNameEndsWithAttribute(
        AttributeSyntax attributeSyntax,
        ServiceLifetime serviceLifetime,
        string suffix)
    : base(
            attributeSyntax: attributeSyntax,
            serviceLifetime: serviceLifetime)
    {
        Suffix = suffix;
        CachedHashCode = new Lazy<int>(() =>
            HashCode
            .Combine(
                base.GetHashCode(),
                Suffix
            )
        );
    }

    public static bool operator ==(
        DeclaredRegisterInterfacesWhereNameEndsWithAttribute left,
        DeclaredRegisterInterfacesWhereNameEndsWithAttribute right)
    =>
        left.Equals(right);

    public static bool operator !=(
        DeclaredRegisterInterfacesWhereNameEndsWithAttribute left,
        DeclaredRegisterInterfacesWhereNameEndsWithAttribute right)
    =>
        !(left == right);

    public override RegisterAttributeOutputBase? CreateOutput(ImmutableArray<INamedTypeSymbol> injectionCandidates)
    {
        throw new NotImplementedException();
    }

    public override bool Equals(object obj) =>
        obj is DeclaredRegisterInterfacesWhereNameEndsWithAttribute other
        && Equals(other);

    public bool Equals(DeclaredRegisterInterfacesWhereNameEndsWithAttribute other) =>
        base.Equals(other)
        && Suffix == other.Suffix;

    public override int GetHashCode() => CachedHashCode.Value;

    public override bool Matches(INamedTypeSymbol typeSymbol) => false;

}
