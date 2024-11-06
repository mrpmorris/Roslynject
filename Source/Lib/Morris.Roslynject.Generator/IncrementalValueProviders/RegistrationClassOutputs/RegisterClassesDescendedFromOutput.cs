using Microsoft.CodeAnalysis;
using Morris.Roslynject.Generator.Extensions;
using Morris.Roslynject.Generator.Helpers;
using System.Collections.Immutable;

namespace Morris.Roslynject.Generator.IncrementalValueProviders.RegistrationClassOutputs;

internal class RegisterClassesDescendedFromOutput : 
    RegisterAttributeOutputBase,
    IEquatable<RegisterClassesDescendedFromOutput>
{
    public readonly INamedTypeSymbol BaseClassType;
    public readonly ImmutableArray<string> ClassesToRegister;
    private readonly Lazy<int> CachedHashCode;

    public static bool operator ==(
        RegisterClassesDescendedFromOutput left,
        RegisterClassesDescendedFromOutput right)
    =>
        left.Equals(right);

    public static bool operator !=(
        RegisterClassesDescendedFromOutput left,
        RegisterClassesDescendedFromOutput right)
    =>
        !(left == right);

    public static RegisterAttributeOutputBase? Create(
        string attributeSourceCode,
        ServiceLifetime serviceLifetime,
        INamedTypeSymbol baseClassType,
        ImmutableArray<INamedTypeSymbol> injectionCandidates)
    {
        ImmutableArray<string> classesToRegister =
            injectionCandidates
            .Where(x => x.DescendsFrom(baseClassType))
            .Select(x =>
                NamespaceHelper.Combine(
                    namespaceSymbol: x.ContainingNamespace,
                    className: x.Name
                )
            )
            .ToImmutableArray();

        return
            classesToRegister.Length == 0
            ? null
            : new RegisterClassesDescendedFromOutput(
                attributeSourceCode: attributeSourceCode,
                serviceLifetime: serviceLifetime,
                baseClassType: baseClassType,
                classesToRegister: classesToRegister);
    }


    public override bool Equals(object obj) =>
        obj is RegisterClassesDescendedFromOutput other
        && Equals(other);

    public bool Equals(RegisterClassesDescendedFromOutput other) =>
        base.Equals(other)
        && TypeIdentityComparer.Default.Equals(
            BaseClassType,
            other.BaseClassType
        )
        && Enumerable.SequenceEqual(
            ClassesToRegister,
            other.ClassesToRegister
        );

    public override void GenerateCode(Action<string> writeLine)
    {
        foreach (string classToRegister in ClassesToRegister)
            writeLine($"services.Add{ServiceLifetime}(typeof(global::{classToRegister}));");
    }
    public override int GetHashCode() => CachedHashCode.Value;

    private RegisterClassesDescendedFromOutput(
        string attributeSourceCode,
        ServiceLifetime serviceLifetime,
        INamedTypeSymbol baseClassType,
        ImmutableArray<string> classesToRegister)
        : base(
            attributeSourceCode: attributeSourceCode,
            serviceLifetime: serviceLifetime)
    {
        BaseClassType = baseClassType;
        ClassesToRegister = classesToRegister;
        CachedHashCode = new Lazy<int>(() =>
            HashCode
            .Combine(
                base.GetHashCode(),
                TypeIdentityComparer.Default.GetHashCode(BaseClassType),
                ClassesToRegister.GetContentsHashCode()
            )
        );
    }

}
