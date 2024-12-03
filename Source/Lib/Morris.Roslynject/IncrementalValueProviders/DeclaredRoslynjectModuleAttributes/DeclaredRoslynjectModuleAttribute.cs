using Morris.Roslynject.Extensions;
using Morris.Roslynject.Helpers;
using System.Collections.Immutable;

namespace Morris.Roslynject.IncrementalValueProviders.DeclaredRoslynjectModuleAttributes;

internal sealed class DeclaredRoslynjectModuleAttribute : IEquatable<DeclaredRoslynjectModuleAttribute>
{
	public readonly string? TargetNamespaceName;
	public readonly string TargetClassName;
	public readonly string TargetFullName;
	public readonly string? ClassRegex;
	public readonly ImmutableArray<DeclaredRoslynjectAttribute> RoslynjectAttributes;

	private readonly Lazy<int> CachedHashCode;

	public DeclaredRoslynjectModuleAttribute(
		string? targetNamespaceName,
		string targetClassName,
		string? classRegex,
		ImmutableArray<DeclaredRoslynjectAttribute> roslynjectAttributes)
	{
		TargetNamespaceName = targetNamespaceName;
		TargetClassName = targetClassName;
		TargetFullName = NamespaceHelper.Combine(targetNamespaceName, targetClassName);
		ClassRegex = classRegex;
		RoslynjectAttributes = roslynjectAttributes;

		CachedHashCode = new Lazy<int>(() =>
			HashCode.Combine(
				TargetNamespaceName,
				TargetClassName,
				ClassRegex,
				RoslynjectAttributes.GetContentsHashCode()
			)
		);
	}

	public bool Equals(DeclaredRoslynjectModuleAttribute other) =>
		string.Equals(TargetNamespaceName, other.TargetNamespaceName, StringComparison.OrdinalIgnoreCase)
		&& string.Equals(TargetClassName, other.TargetClassName, StringComparison.OrdinalIgnoreCase)
		&& string.Equals(ClassRegex!, other.ClassRegex!, StringComparison.OrdinalIgnoreCase)
		&& Enumerable.SequenceEqual(RoslynjectAttributes, other.RoslynjectAttributes);

	public override bool Equals(object obj) =>
		obj is DeclaredRoslynjectModuleAttribute other
		&& other.Equals(this);

	public override int GetHashCode() => CachedHashCode.Value;
}
