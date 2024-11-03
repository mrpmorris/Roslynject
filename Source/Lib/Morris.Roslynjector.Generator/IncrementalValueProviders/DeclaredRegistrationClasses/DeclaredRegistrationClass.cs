using Microsoft.CodeAnalysis;
using Morris.Roslynjector.Generator.Extensions;
using Morris.Roslynjector.Generator.Helpers;
using System.Collections.Immutable;

namespace Morris.Roslynjector.Generator.IncrementalValueProviders.DeclaredRegistrationClasses;

internal class DeclaredRegistrationClass : IEquatable<DeclaredRegistrationClass>
{
    public readonly string ClassName;
    public readonly string? NamespaceName;
    public readonly ImmutableArray<DeclaredRegisterAttributeBase> Attributes;

    public string FullName => NamespaceHelper.Combine(NamespaceName, ClassName);

    private readonly Lazy<int> CachedHashCode;

    public DeclaredRegistrationClass(
        string? namespaceName,
        string className,
        ImmutableArray<DeclaredRegisterAttributeBase> attributes)
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

    public static bool operator ==(DeclaredRegistrationClass left, DeclaredRegistrationClass right) => left.Equals(right);
    public static bool operator !=(DeclaredRegistrationClass left, DeclaredRegistrationClass right) => !(left == right);
    public override bool Equals(object obj) => obj is DeclaredRegistrationClass other && Equals(other);

    public bool Equals(DeclaredRegistrationClass other) =>
        ReferenceEquals(this, other)
        ||

            ClassName == other.ClassName
            && NamespaceName == other.NamespaceName
            && Attributes.SequenceEqual(other.Attributes)
        ;

    public override int GetHashCode() => CachedHashCode.Value;
}

