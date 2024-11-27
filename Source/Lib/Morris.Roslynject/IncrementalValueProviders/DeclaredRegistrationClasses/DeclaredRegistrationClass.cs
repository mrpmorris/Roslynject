using Microsoft.CodeAnalysis;
using Morris.Roslynject.Extensions;
using Morris.Roslynject.Helpers;
using Morris.Roslynject.IncrementalValueProviders.RegistrationClassOutputs;
using System.Collections.Immutable;

namespace Morris.Roslynject.IncrementalValueProviders.DeclaredRegistrationClasses;

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

	public RegistrationClassOutput CreateOutput(
		ImmutableArray<INamedTypeSymbol> injectionCandidates)
	{
		var attributes =
			Attributes
			.Select(x => x.CreateOutput(injectionCandidates)!)
			.Where(x => x is not null)
			.ToImmutableArray();

		return new RegistrationClassOutput(
			namespaceName: NamespaceName,
			className: ClassName,
			attributes: attributes
		);
	}

	public override bool Equals(object? obj) =>
		obj is DeclaredRegistrationClass other
		&& Equals(other);

	public bool Equals(DeclaredRegistrationClass? other) =>
		ReferenceEquals(this, other)
		||
			other is not null
			&& ClassName == other.ClassName
			&& NamespaceName == other.NamespaceName
			&& Attributes.SequenceEqual(other.Attributes)
		;

	public override int GetHashCode() => CachedHashCode.Value;

}

