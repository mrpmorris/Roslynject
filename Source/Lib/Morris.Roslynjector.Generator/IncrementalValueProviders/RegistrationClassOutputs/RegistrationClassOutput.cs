using Microsoft.CodeAnalysis;
using Morris.Roslynjector.Generator.Extensions;
using Morris.Roslynjector.Generator.Helpers;
using Morris.Roslynjector.Generator.IncrementalValueProviders.DeclaredRegistrationClasses;
using System.Collections.Immutable;

namespace Morris.Roslynjector.Generator.IncrementalValueProviders.RegistrationClassOutputs;

internal class RegistrationClassOutput : IEquatable<RegistrationClassOutput>
{
    public readonly string ClassName;
    public readonly string? NamespaceName;
    public readonly ImmutableArray<RegisterAttributeOutputBase> Attributes;

    public string FullName => NamespaceHelper.Combine(NamespaceName, ClassName);

    private readonly Lazy<int> CachedHashCode;

    public RegistrationClassOutput(
        string? namespaceName,
        string className,
        ImmutableArray<RegisterAttributeOutputBase> attributes)
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

    public static bool operator ==(RegistrationClassOutput left, RegistrationClassOutput right) => left.Equals(right);
    public static bool operator !=(RegistrationClassOutput left, RegistrationClassOutput right) => !(left == right);

    public override bool Equals(object obj) =>
        obj is RegistrationClassOutput other
        && Equals(other);

    public bool Equals(RegistrationClassOutput other) =>
        ReferenceEquals(this, other)
        ||

            ClassName == other.ClassName
            && NamespaceName == other.NamespaceName
            && Attributes.SequenceEqual(other.Attributes)
        ;

    public override int GetHashCode() => CachedHashCode.Value;

}

