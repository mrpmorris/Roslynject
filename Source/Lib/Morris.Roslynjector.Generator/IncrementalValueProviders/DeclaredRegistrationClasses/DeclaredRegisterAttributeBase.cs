using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Morris.Roslynjector.Generator.IncrementalValueProviders.RegistrationClassOutputs;
using System.Collections.Immutable;

namespace Morris.Roslynjector.Generator.IncrementalValueProviders.DeclaredRegistrationClasses;

internal abstract class DeclaredRegisterAttributeBase
{
    public readonly AttributeSyntax AttributeSyntax;
    public readonly ServiceLifetime ServiceLifetime;

    public abstract bool Matches(INamedTypeSymbol typeSymbol);

    public abstract RegisterAttributeOutputBase? CreateOutput(ImmutableArray<INamedTypeSymbol> injectionCandidates);

    protected DeclaredRegisterAttributeBase(
        AttributeSyntax attributeSyntax,
        ServiceLifetime serviceLifetime)
    {
        AttributeSyntax = attributeSyntax;
        ServiceLifetime = serviceLifetime;
    }
}
