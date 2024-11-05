using Microsoft.CodeAnalysis;
using Morris.Roslynject.Generator.Extensions;
using System.Collections.Immutable;

namespace Morris.Roslynject.Generator.IncrementalValueProviders.RegistrationClassOutputs;

internal class RegisterInterfacesOfTypeAttributeOutput :
    RegisterAttributeOutputBase,
    IEquatable<RegisterInterfacesOfTypeAttributeOutput>
{
    public readonly INamedTypeSymbol BaseInterfaceType;
    public readonly ImmutableArray<(string interfaceName, string className)> ClassesToRegister;
    private readonly Lazy<int> CachedHashCode;

    public static bool operator ==(
        RegisterInterfacesOfTypeAttributeOutput left,
        RegisterInterfacesOfTypeAttributeOutput right)
    =>
        left.Equals(right);

    public static bool operator !=(
        RegisterInterfacesOfTypeAttributeOutput left,
        RegisterInterfacesOfTypeAttributeOutput right)
    =>
        !left.Equals(right);

    public static RegisterAttributeOutputBase? Create(
        string attributeSourceCode,
        ServiceLifetime serviceLifetime,
        INamedTypeSymbol baseInterfaceType,
        ImmutableArray<INamedTypeSymbol> injectionCandidates)
    {
        ImmutableArray<(string InterfaceName, string ClassName)> classesToRegister =
            injectionCandidates
            .SelectMany(
                x => x.Interfaces,
                (injectionCandidate, interfaceType) => (injectionCandidate, interfaceType)
            )
            .Where(x => x.interfaceType.IsOfType(baseInterfaceType))
            .Select(x =>
                (
                    x.interfaceType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
                    x.injectionCandidate.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                )
             )
            .ToImmutableArray();

        return
            classesToRegister.Length == 0
            ? null
            : new RegisterInterfacesOfTypeAttributeOutput(
                attributeSourceCode: attributeSourceCode,
                serviceLifetime: serviceLifetime,
                baseInterfaceType: baseInterfaceType,
                classesToRegister: classesToRegister);
    }

    public override bool Equals(object obj) =>
        obj is RegisterInterfacesOfTypeAttributeOutput other
        && Equals(other);

    public bool Equals(RegisterInterfacesOfTypeAttributeOutput other) =>
        base.Equals(other)
        && TypeIdentityComparer.Default.Equals(
            BaseInterfaceType,
            other.BaseInterfaceType
        )
        && Enumerable.SequenceEqual(
            ClassesToRegister,
            other.ClassesToRegister
        );

    public override void GenerateCode(Action<string> writeLine)
    {
        foreach ((string InterfaceName, string ClassName) classToRegister in ClassesToRegister)
            writeLine($"services.Add{ServiceLifetime}(typeof({classToRegister.InterfaceName}), typeof({classToRegister.ClassName}));");
    }

    public override int GetHashCode() => CachedHashCode.Value;

    private RegisterInterfacesOfTypeAttributeOutput(
        string attributeSourceCode,
        ServiceLifetime serviceLifetime,
        INamedTypeSymbol baseInterfaceType,
        ImmutableArray<(string interfaceName, string className)> classesToRegister)
    : base(
        attributeSourceCode: attributeSourceCode,
        serviceLifetime: serviceLifetime)
    {
        BaseInterfaceType = baseInterfaceType;
        ClassesToRegister = classesToRegister;
        CachedHashCode = new Lazy<int>(() =>
            HashCode
            .Combine(
                base.GetHashCode(),
                TypeIdentityComparer.Default.GetHashCode(BaseInterfaceType),
                ClassesToRegister.GetContentsHashCode()
            )
        );
    }

}
