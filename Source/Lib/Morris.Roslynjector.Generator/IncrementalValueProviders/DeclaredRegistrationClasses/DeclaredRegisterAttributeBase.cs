using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace Morris.Roslynjector.Generator.IncrementalValueProviders.DeclaredRegistrationClasses;

internal abstract class DeclaredRegisterAttributeBase
{
    public readonly ServiceLifetime ServiceLifetime;

    public abstract bool Matches(INamedTypeSymbol typeSymbol);

    protected DeclaredRegisterAttributeBase(ServiceLifetime serviceLifetime)
    {
        ServiceLifetime = serviceLifetime;
    }
}
