using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace Morris.Roslynjector.Generator.IncrementalValueProviders.RegistrationClassOutputs;

internal class RegisterClassesWhereDescendsFromAttributeOutput : 
    RegisterAttributeOutputBase,
    IEquatable<RegisterClassesWhereDescendsFromAttributeOutput>
{
    public readonly INamedTypeSymbol BaseClassType;
    public readonly ImmutableArray<INamedTypeSymbol> ClassesToRegister;

    public RegisterClassesWhereDescendsFromAttributeOutput(
        ServiceLifetime serviceLifetime,
        INamedTypeSymbol baseClassType,
        ImmutableArray<INamedTypeSymbol> injectionCandidates)
        : base(serviceLifetime)
    {
        BaseClassType = baseClassType;
        ClassesToRegister = injectionCandidates;
    }

    public bool Equals(RegisterClassesWhereDescendsFromAttributeOutput other)
    {
        throw new NotImplementedException();
    }

    public override void GenerateCode(Action<string> writeLine)
    {
        throw new NotImplementedException();
    }
}
