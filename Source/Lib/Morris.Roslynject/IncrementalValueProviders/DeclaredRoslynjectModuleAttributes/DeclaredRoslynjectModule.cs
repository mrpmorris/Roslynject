using Microsoft.CodeAnalysis;
using Morris.Roslynject.Extensions;
using Morris.Roslynject.Helpers;
using System.Collections.Immutable;

namespace Morris.Roslynject.IncrementalValueProviders.DeclaredRoslynjectModuleAttributes;

internal sealed class DeclaredRoslynjectModule : IEquatable<DeclaredRoslynjectModule>
{
	public readonly string? TargetNamespaceName;
	public readonly string TargetClassName;
	public readonly string TargetFullName;
	public readonly string? ClassRegex;
	public readonly ImmutableArray<DeclaredRoslynjectAttribute> RoslynjectAttributes;

	private readonly Lazy<int> CachedHashCode;
	private readonly Lazy<Func<INamedTypeSymbol, bool>> ClassRegexMatches;

	public DeclaredRoslynjectModule(
		string? targetNamespaceName,
		string targetClassName,
		string? classRegex,
		IEnumerable<DeclaredRoslynjectAttribute> roslynjectAttributes)
	{
		TargetNamespaceName = targetNamespaceName;
		TargetClassName = targetClassName;
		TargetFullName = NamespaceHelper.Combine(targetNamespaceName, targetClassName);
		ClassRegex = classRegex;
		RoslynjectAttributes = roslynjectAttributes.ToImmutableArray();

		ClassRegexMatches = TypeNamePredicate.Create(ClassRegex);
		CachedHashCode = new Lazy<int>(() =>
			HashCode.Combine(
				TargetNamespaceName,
				TargetClassName,
				ClassRegex,
				RoslynjectAttributes.GetContentsHashCode()
			)
		);
	}

	public bool Equals(DeclaredRoslynjectModule other) =>
		string.Equals(TargetNamespaceName, other.TargetNamespaceName, StringComparison.OrdinalIgnoreCase)
		&& string.Equals(TargetClassName, other.TargetClassName, StringComparison.OrdinalIgnoreCase)
		&& string.Equals(ClassRegex!, other.ClassRegex!, StringComparison.OrdinalIgnoreCase)
		&& Enumerable.SequenceEqual(RoslynjectAttributes, other.RoslynjectAttributes);

	public override bool Equals(object obj) =>
		obj is DeclaredRoslynjectModule other
		&& other.Equals(this);

	public override int GetHashCode() => CachedHashCode.Value;

	public bool IsMatch(INamedTypeSymbol symbol) =>
		ClassRegexMatches.Value(symbol);
}
