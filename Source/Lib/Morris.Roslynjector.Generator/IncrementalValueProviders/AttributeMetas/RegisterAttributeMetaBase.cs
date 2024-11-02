using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace Morris.Roslynjector.Generator.IncrementalValueProviders.AttributeMetas;

internal abstract class RegisterAttributeMetaBase
{
    public readonly ServiceLifetime ServiceLifetime;
    public readonly ImmutableArray<INamedTypeSymbol> ClassesToRegister;

    public abstract bool Matches(INamedTypeSymbol typeSymbol);

    public abstract RegisterAttributeMetaBase CloneWithClassesToRegister(
        ImmutableArray<INamedTypeSymbol> classes);

    public abstract void GenerateCode(Action<string> writeLine);

    protected RegisterAttributeMetaBase(
        ServiceLifetime serviceLifetime,
        ImmutableArray<INamedTypeSymbol> classesToRegister)
    {
        ServiceLifetime = serviceLifetime;
        ClassesToRegister = classesToRegister;
    }

}
