using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Morris.Roslynjector.Generator.IncrementalValueProviders.RegistrationClassOutputs;
using System.Collections.Immutable;

namespace Morris.Roslynjector.Generator.IncrementalValueProviders.DeclaredRegistrationClasses;

internal abstract class DeclaredRegisterAttributeBase
{
    public readonly string AttributeSourceCode;
    public readonly ServiceLifetime ServiceLifetime;
    private readonly Lazy<int> CachedHashCode;

    public abstract bool Matches(INamedTypeSymbol typeSymbol);

    public abstract RegisterAttributeOutputBase? CreateOutput(ImmutableArray<INamedTypeSymbol> injectionCandidates);

    public override bool Equals(object obj) => 
        obj is RegisterAttributeOutputBase other
        && ServiceLifetime == other.ServiceLifetime
        && AttributeSourceCode.Equals(
            other.AttributeSourceCode,
            StringComparison.Ordinal
        );

    public override int GetHashCode() => CachedHashCode.Value;

    protected DeclaredRegisterAttributeBase(
        AttributeSyntax attributeSyntax,
        ServiceLifetime serviceLifetime)
    {
        AttributeSourceCode = attributeSyntax
            .ToFullString()
            .Replace("\r\n", " ")
            .Replace('\n', ' ');
        ServiceLifetime = serviceLifetime;
        CachedHashCode = new Lazy<int>(() =>
            HashCode
            .Combine(
                ServiceLifetime,
                AttributeSourceCode
            )
        );
    }
}
