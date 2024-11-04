using Microsoft.CodeAnalysis;
using Morris.Roslynjector.Generator.Extensions;
using Morris.Roslynjector.Generator.Helpers;
using Morris.Roslynjector.Generator.IncrementalValueProviders.DeclaredRegistrationClasses;
using System.Collections.Immutable;

namespace Morris.Roslynjector.Generator.IncrementalValueProviders.RegistrationClassOutputs;

internal class RegisterClassesWhereNameEndsWithAttributeOutput :
    RegisterAttributeOutputBase,
    IEquatable<RegisterClassesWhereNameEndsWithAttributeOutput>
{
    public readonly ImmutableArray<string> ClassesToRegister;
    private readonly Lazy<int> CachedHashCode;

    public static bool operator ==(RegisterClassesWhereNameEndsWithAttributeOutput left, RegisterClassesWhereNameEndsWithAttributeOutput right) => left.Equals(right);
    public static bool operator !=(RegisterClassesWhereNameEndsWithAttributeOutput left, RegisterClassesWhereNameEndsWithAttributeOutput right) => !(left == right);

    public static RegisterClassesWhereNameEndsWithAttributeOutput? Create(
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
                serviceLifetime: serviceLifetime,
                classesToRegister: classesToRegister);
    }

    public override bool Equals(object obj) =>
        obj is RegisterClassesWhereNameEndsWithAttributeOutput other
        && Equals(other);

    public bool Equals(RegisterClassesWhereNameEndsWithAttributeOutput other) =>
        ServiceLifetime == other.ServiceLifetime
        && Enumerable.SequenceEqual(
            ClassesToRegister,
            other.ClassesToRegister
        );

    public override void GenerateCode(Action<string> writeLine)
    {
        foreach (string classToRegister in ClassesToRegister)
            writeLine($"services.Add{ServiceLifetime}(global::{classToRegister});");
    }

    public override int GetHashCode() => CachedHashCode.Value;

    private RegisterClassesWhereNameEndsWithAttributeOutput(
        ServiceLifetime serviceLifetime,
        ImmutableArray<string> classesToRegister)
        : base(serviceLifetime)
    {
        ClassesToRegister = classesToRegister;
        CachedHashCode = new Lazy<int>(() =>
            HashCode
            .Combine(
                ServiceLifetime,
                ClassesToRegister.GetContentsHashCode()
            )
        );
    }
}
