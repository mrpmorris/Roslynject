﻿using Microsoft.CodeAnalysis;
using Morris.Roslynject.Extensions;
using Morris.Roslynject.IncrementalValueProviders.DeclaredRegistrationClasses;
using Morris.Roslynject.Helpers;
using System.Collections.Immutable;

namespace Morris.Roslynject.IncrementalValueProviders.RegistrationClassOutputs;

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

	public override bool Equals(object? obj) =>
		obj is RegistrationClassOutput other
		&& Equals(other);

	public bool Equals(RegistrationClassOutput? other) =>
		ReferenceEquals(this, other)
		||
			other is not null
			&& ClassName == other.ClassName
			&& NamespaceName == other.NamespaceName
			&& Attributes.SequenceEqual(other.Attributes)
		;

	public override int GetHashCode() => CachedHashCode.Value;

}
