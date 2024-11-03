using Microsoft.CodeAnalysis;
using Morris.Roslynjector.Generator.Extensions;
using Morris.Roslynjector.Generator.Helpers;
using Morris.Roslynjector.Generator.IncrementalValueProviders.AttributeMetas;
using System.Collections.Immutable;

namespace Morris.Roslynjector.Generator.IncrementalValueProviders.DiscoveredRegistrationClasses;

internal class DiscoveredRegistrationClass : IEquatable<DiscoveredRegistrationClass>
{
    public readonly string ClassName;
    public readonly string? NamespaceName;
    public readonly ImmutableArray<RegisterAttributeMetaBase> Attributes;

    public string FullName => NamespaceHelper.Combine(NamespaceName, ClassName);

    private readonly Lazy<int> CachedHashCode;

    public DiscoveredRegistrationClass(
        string? namespaceName,
        string className,
        ImmutableArray<RegisterAttributeMetaBase> attributes)
    {
        NamespaceName = namespaceName;
        ClassName = className;
        Attributes = attributes;

        CachedHashCode = new Lazy<int>(() =>
            HashCode
            .Combine(
                className,
                namespaceName,
                Attributes.GetContentsHashCode()));
    }

    public static bool operator ==(DiscoveredRegistrationClass left, DiscoveredRegistrationClass right) => left.Equals(right);
    public static bool operator !=(DiscoveredRegistrationClass left, DiscoveredRegistrationClass right) => !(left == right);
    public override bool Equals(object obj) => obj is DiscoveredRegistrationClass other && Equals(other);

    public DiscoveredRegistrationClass CloneWithCandidateClasses(ImmutableArray<INamedTypeSymbol> classes)
    =>
        new DiscoveredRegistrationClass(
            namespaceName: NamespaceName,
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

    public bool Equals(DiscoveredRegistrationClass other) =>
        ReferenceEquals(this, other)
        ||

            ClassName == other.ClassName
            && NamespaceName == other.NamespaceName
            && Attributes.SequenceEqual(other.Attributes)
        ;

    public override int GetHashCode() => CachedHashCode.Value;
}

