using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Morris.Roslynject.Generator.Extensions;
using Morris.Roslynject.Generator.Helpers;
using Morris.Roslynject.Generator.IncrementalValueProviders.DeclaredRegistrationClasses;
using System.Collections.Immutable;

namespace Morris.Roslynject.Generator.IncrementalValueProviders.RegistrationClassOutputs;

internal class RegisterClassesWhereDescendsFromAttributeOutput : 
    RegisterAttributeOutputBase,
    IEquatable<RegisterClassesWhereDescendsFromAttributeOutput>
{
    public readonly INamedTypeSymbol BaseClassType;
    public readonly ImmutableArray<string> ClassesToRegister;
    private readonly Lazy<int> CachedHashCode;

    public static bool operator ==(
        RegisterClassesWhereDescendsFromAttributeOutput left,
        RegisterClassesWhereDescendsFromAttributeOutput right)
    =>
        left.Equals(right);

    public static bool operator !=(
        RegisterClassesWhereDescendsFromAttributeOutput left,
        RegisterClassesWhereDescendsFromAttributeOutput right)
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
            : new RegisterClassesWhereDescendsFromAttributeOutput(
                attributeSourceCode: attributeSourceCode,
                serviceLifetime: serviceLifetime,
                baseClassType: baseClassType,
                classesToRegister: classesToRegister);
    }

    public override int GetHashCode() => CachedHashCode.Value;

    public override bool Equals(object obj) =>
        obj is RegisterClassesWhereDescendsFromAttributeOutput other
        && Equals(other);

    public bool Equals(RegisterClassesWhereDescendsFromAttributeOutput other) =>
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

    private RegisterClassesWhereDescendsFromAttributeOutput(
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
