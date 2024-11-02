using Microsoft.CodeAnalysis;
using Morris.Roslynjector.Generator.Extensions;
using Morris.Roslynjector.Generator.Helpers;
using Morris.Roslynjector.Generator.IncrementalValueProviders.AttributeMetas;
using System.Collections.Immutable;

namespace Morris.Roslynjector.Generator.IncrementalValueProviders.RegistrationClassMetas;

internal class RegistrationClassMeta : IEquatable<RegistrationClassMeta>
{
    public readonly string ClassName;
    public readonly string? Namespace;
    public readonly ImmutableArray<RegisterAttributeMetaBase> Attributes;

    public string FullName => NamespaceHelper.Combine(Namespace, ClassName);

    private readonly Lazy<int> CachedHashCode;

    public RegistrationClassMeta(
        string? @namespace,
        string className,
        ImmutableArray<RegisterAttributeMetaBase> attributes)
    {
        Namespace = @namespace;
        ClassName = className;
        Attributes = attributes;

        CachedHashCode = new Lazy<int>(() =>
            HashCode
            .Combine(
                className,
                @namespace,
                Attributes.GetContentsHashCode()));
    }

    public static bool operator ==(RegistrationClassMeta left, RegistrationClassMeta right) => left.Equals(right);
    public static bool operator !=(RegistrationClassMeta left, RegistrationClassMeta right) => !(left == right);
    public override bool Equals(object obj) => obj is RegistrationClassMeta other && Equals(other);

    public RegistrationClassMeta CloneWithCandidateClasses(ImmutableArray<INamedTypeSymbol> classes)
    =>
        new RegistrationClassMeta(
            @namespace: Namespace,
            className: ClassName,
            attributes:
                Attributes
                .Select(x => x
                    .CloneWithClassesToRegister(
                        classes
                        .Where(c => x.Matches(c))
                        .ToImmutableArray()
                    )
                )
                .Where(x => x.ClassesToRegister.Length > 0)
                .Distinct()
                .ToImmutableArray()
        );

    public bool Equals(RegistrationClassMeta other) =>
        ReferenceEquals(this, other)
        ||

            ClassName == other.ClassName
            && Namespace == other.Namespace
            && Attributes.SequenceEqual(other.Attributes)
        ;

    public override int GetHashCode() => CachedHashCode.Value;
}

