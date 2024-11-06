using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Morris.Roslynject.Generator.Extensions;
using Morris.Roslynject.Generator.IncrementalValueProviders.RegistrationClassOutputs;
using System.Collections.Immutable;

namespace Morris.Roslynject.Generator.IncrementalValueProviders.DeclaredRegistrationClasses;

internal class DeclaredRegisterClassesDescendedFromAttribute : DeclaredRegisterAttributeBase, IEquatable<DeclaredRegisterClassesDescendedFromAttribute>
{
    public readonly INamedTypeSymbol BaseClassType;
    private readonly Lazy<int> CachedHashCode;

    public DeclaredRegisterClassesDescendedFromAttribute(
        AttributeSyntax attributeSyntax,
        ServiceLifetime serviceLifetime,
        INamedTypeSymbol baseClassType)
        : base(
            attributeSyntax: attributeSyntax,
            serviceLifetime: serviceLifetime)
    {
        BaseClassType = baseClassType;
        CachedHashCode = new Lazy<int>(() =>
            HashCode
            .Combine(
                base.GetHashCode(),
                BaseClassType.ToDisplayString()
             )
        );
    }

    public static bool operator ==(
        DeclaredRegisterClassesDescendedFromAttribute left,
        DeclaredRegisterClassesDescendedFromAttribute right)
    =>
        left.Equals(right);

    public static bool operator !=(
        DeclaredRegisterClassesDescendedFromAttribute left,
        DeclaredRegisterClassesDescendedFromAttribute right)
    =>
        !(left == right);

    public override RegisterAttributeOutputBase? CreateOutput(
        ImmutableArray<INamedTypeSymbol> injectionCandidates)
    =>
        RegisterClassesDescendedFromOutput.Create(
            attributeSourceCode: AttributeSourceCode,
            serviceLifetime: ServiceLifetime,
            baseClassType: BaseClassType,
            injectionCandidates: injectionCandidates);

    public override bool Equals(object obj) =>
        obj is DeclaredRegisterClassesDescendedFromAttribute other
        && Equals(other);

    public bool Equals(DeclaredRegisterClassesDescendedFromAttribute other) =>
        base.Equals(other)
        && TypeIdentifyWithInheritanceComparer.Default.Equals(BaseClassType, other.BaseClassType);

    public override int GetHashCode() => CachedHashCode.Value;

    public override bool Matches(INamedTypeSymbol typeSymbol) =>
        typeSymbol.DescendsFrom(BaseClassType);
}
