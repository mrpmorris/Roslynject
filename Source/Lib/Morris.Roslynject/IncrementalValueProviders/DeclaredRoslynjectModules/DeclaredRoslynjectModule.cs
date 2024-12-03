namespace Morris.Roslynject.IncrementalValueProviders.DeclaredRoslynjectModules;

internal sealed class DeclaredRoslynjectModule : IEquatable<DeclaredRoslynjectModule>
{
	public readonly string? TargetNamespaceName;
	public readonly string TargetClassName;
	public readonly string? ClassRegex;

	private readonly Lazy<int> CachedHashCode;

	public DeclaredRoslynjectModule(
		string? targetNamespaceName,
		string targetClassName,
		string? classRegex)
	{
		TargetNamespaceName = targetNamespaceName;
		TargetClassName = targetClassName;
		ClassRegex = classRegex;

		CachedHashCode = new Lazy<int>(() =>
			HashCode.Combine(
				TargetNamespaceName,
				TargetClassName,
				ClassRegex
			)
		);
	}

	public bool Equals(DeclaredRoslynjectModule other) =>
		string.Equals(TargetNamespaceName, other.TargetNamespaceName, StringComparison.OrdinalIgnoreCase)
		&& string.Equals(TargetClassName, other.TargetClassName, StringComparison.OrdinalIgnoreCase)
		&& string.Equals(ClassRegex!, other.ClassRegex!, StringComparison.OrdinalIgnoreCase);

	public override bool Equals(object obj) =>
		obj is DeclaredRoslynjectModule other
		&& other.Equals(this);

	public override int GetHashCode() => CachedHashCode.Value;
}
