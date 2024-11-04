using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Morris.Roslynject.Generator.IncrementalValueProviders.RegistrationClassOutputs;
using System.Collections.Immutable;

namespace Morris.Roslynject.Generator.IncrementalValueProviders.DeclaredRegistrationClasses;

internal class DeclaredRegisterClassesWhereNameEndsWithAttribute : DeclaredRegisterAttributeBase, IEquatable<DeclaredRegisterClassesWhereNameEndsWithAttribute>
{
    public readonly string Suffix;
    private readonly Lazy<int> CachedHashCode;

    public DeclaredRegisterClassesWhereNameEndsWithAttribute(
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
        DeclaredRegisterClassesWhereNameEndsWithAttribute left,
        DeclaredRegisterClassesWhereNameEndsWithAttribute right)
    =>
        left.Equals(right);

    public static bool operator !=(
        DeclaredRegisterClassesWhereNameEndsWithAttribute left,
        DeclaredRegisterClassesWhereNameEndsWithAttribute right)
    =>
        !(left == right);

    public override RegisterAttributeOutputBase? CreateOutput(
        ImmutableArray<INamedTypeSymbol> injectionCandidates)
    =>
        RegisterClassesWhereNameEndsWithAttributeOutput.Create(
            attributeSourceCode: AttributeSourceCode,
            serviceLifetime: ServiceLifetime,
            suffix: Suffix,
            injectionCandidates: injectionCandidates);

    public override bool Equals(object obj) =>
        obj is DeclaredRegisterClassesWhereNameEndsWithAttribute other
        && Equals(other);

    public bool Equals(DeclaredRegisterClassesWhereNameEndsWithAttribute other) =>
        base.Equals(other)
        && Suffix == other.Suffix;

    public override int GetHashCode() => CachedHashCode.Value;

    public override bool Matches(INamedTypeSymbol typeSymbol) =>
        typeSymbol.Name.EndsWith(Suffix, StringComparison.OrdinalIgnoreCase);
}
