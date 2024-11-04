using Microsoft.CodeAnalysis;
using Morris.Roslynject.Generator.Extensions;
using Morris.Roslynject.Generator.Helpers;
using System.Collections.Immutable;

namespace Morris.Roslynject.Generator.IncrementalValueProviders.RegistrationClassOutputs;

internal class RegisterClassesWhereNameEndsWithAttributeOutput :
    RegisterAttributeOutputBase,
    IEquatable<RegisterClassesWhereNameEndsWithAttributeOutput>
{
    public readonly ImmutableArray<string> ClassesToRegister;
    private readonly Lazy<int> CachedHashCode;

    public static bool operator ==(
        RegisterClassesWhereNameEndsWithAttributeOutput left,
        RegisterClassesWhereNameEndsWithAttributeOutput right)
    =>
        left.Equals(right);

    public static bool operator !=(
        RegisterClassesWhereNameEndsWithAttributeOutput left,
        RegisterClassesWhereNameEndsWithAttributeOutput right)
    =>
        !(left == right);

    public static RegisterClassesWhereNameEndsWithAttributeOutput? Create(
        string attributeSourceCode,
        ServiceLifetime serviceLifetime,
        string suffix,
        ImmutableArray<INamedTypeSymbol> injectionCandidates)
    {
        ImmutableArray<string> classesToRegister =
            injectionCandidates
            .Where(x => x.Name.EndsWith(suffix, StringComparison.Ordinal))
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
            : new RegisterClassesWhereNameEndsWithAttributeOutput(
                attributeSourceCode: attributeSourceCode,
                serviceLifetime: serviceLifetime,
                classesToRegister: classesToRegister);
    }

    public override bool Equals(object obj) =>
        obj is RegisterClassesWhereNameEndsWithAttributeOutput other
        && Equals(other);

    public bool Equals(RegisterClassesWhereNameEndsWithAttributeOutput other) =>
        base.Equals(other)
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

    private RegisterClassesWhereNameEndsWithAttributeOutput(
        string attributeSourceCode,
        ServiceLifetime serviceLifetime,
        ImmutableArray<string> classesToRegister)
        : base(
            attributeSourceCode: attributeSourceCode,
            serviceLifetime: serviceLifetime)
    {
        ClassesToRegister = classesToRegister;
        CachedHashCode = new Lazy<int>(() =>
            HashCode
            .Combine(
                base.GetHashCode(),
                ClassesToRegister.GetContentsHashCode()
            )
        );
    }
}
