using Morris.Roslynject.Helpers;

namespace Morris.Roslynject.IncrementalValueProviders.DeclaredRoslynjectModuleAttributes;

internal sealed class DeclaredRoslynjectModuleAttribute : IEquatable<DeclaredRoslynjectModuleAttribute>
{
	public readonly string? TargetNamespaceName;
	public readonly string TargetClassName;
	public readonly string TargetFullName;
	public readonly string? ClassRegex;

	private readonly Lazy<int> CachedHashCode;

	public DeclaredRoslynjectModuleAttribute(
		string? targetNamespaceName,
		string targetClassName,
		string? classRegex)
	{
		TargetNamespaceName = targetNamespaceName;
		TargetClassName = targetClassName;
		TargetFullName = NamespaceHelper.Combine(targetNamespaceName, targetClassName);
		ClassRegex = classRegex;

		CachedHashCode = new Lazy<int>(() =>
			HashCode.Combine(
				TargetNamespaceName,
				TargetClassName,
				ClassRegex
			)
		);
	}

	public bool Equals(DeclaredRoslynjectModuleAttribute other) =>
		string.Equals(TargetNamespaceName, other.TargetNamespaceName, StringComparison.OrdinalIgnoreCase)
		&& string.Equals(TargetClassName, other.TargetClassName, StringComparison.OrdinalIgnoreCase)
		&& string.Equals(ClassRegex!, other.ClassRegex!, StringComparison.OrdinalIgnoreCase);

	public override bool Equals(object obj) =>
		obj is DeclaredRoslynjectModuleAttribute other
		&& other.Equals(this);

	public override int GetHashCode() => CachedHashCode.Value;
}
