using Microsoft.CodeAnalysis;
using Morris.Roslynjector.Generator.IncrementalValueProviders.RegistrationClassOutputs;
using System.Collections.Immutable;

namespace Morris.Roslynjector.Generator.IncrementalValueProviders.DeclaredRegistrationClasses;

internal abstract class DeclaredRegisterAttributeBase
{
    public readonly ServiceLifetime ServiceLifetime;

    public abstract bool Matches(INamedTypeSymbol typeSymbol);

    public abstract RegisterAttributeOutputBase CreateOutput(ImmutableArray<INamedTypeSymbol> injectionCandidates);

    protected DeclaredRegisterAttributeBase(ServiceLifetime serviceLifetime)
    {
        ServiceLifetime = serviceLifetime;
    }
}
