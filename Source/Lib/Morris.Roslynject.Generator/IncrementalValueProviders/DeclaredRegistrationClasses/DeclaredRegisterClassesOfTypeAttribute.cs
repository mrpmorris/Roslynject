using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Morris.Roslynject.Generator.Extensions;
using Morris.Roslynject.Generator.IncrementalValueProviders.RegistrationClassOutputs;
using System.Collections.Immutable;

namespace Morris.Roslynject.Generator.IncrementalValueProviders.DeclaredRegistrationClasses;

internal class DeclaredRegisterClassesOfTypeAttribute : DeclaredRegisterAttributeBase, IEquatable<DeclaredRegisterClassesOfTypeAttribute>
{
    public readonly INamedTypeSymbol BaseClassType;
    private readonly Lazy<int> CachedHashCode;

    public DeclaredRegisterClassesOfTypeAttribute(
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
        DeclaredRegisterClassesOfTypeAttribute left,
        DeclaredRegisterClassesOfTypeAttribute right)
    =>
        left.Equals(right);

    public static bool operator !=(
        DeclaredRegisterClassesOfTypeAttribute left,
        DeclaredRegisterClassesOfTypeAttribute right)
    =>
        !(left == right);

    public override RegisterAttributeOutputBase? CreateOutput(
        ImmutableArray<INamedTypeSymbol> injectionCandidates)
    =>
        RegisterClassesOfTypeOutput.Create(
            attributeSourceCode: AttributeSourceCode,
            serviceLifetime: ServiceLifetime,
            baseClassType: BaseClassType,
            injectionCandidates: injectionCandidates);

    public override bool Equals(object obj) =>
        obj is DeclaredRegisterClassesOfTypeAttribute other
        && Equals(other);

    public bool Equals(DeclaredRegisterClassesOfTypeAttribute other) =>
        base.Equals(other)
        && TypeIdentifyWithInheritanceComparer.Default.Equals(BaseClassType, other.BaseClassType);

    public override int GetHashCode() => CachedHashCode.Value;

    public override bool Matches(INamedTypeSymbol typeSymbol) =>
        typeSymbol.DescendsFrom(BaseClassType);
}
