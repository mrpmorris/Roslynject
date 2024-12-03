namespace Morris.Roslynject.IncrementalValueProviders.DeclaredRoslynjectModules;

internal sealed class DeclaredRoslynjectModule : IEquatable<DeclaredRoslynjectModule>
{
	public readonly string TargetFullName;
	public readonly string? ClassRegex;

	private readonly Lazy<int> CachedHashCode;

	public DeclaredRoslynjectModule(
		string targetFullName,
		string? classRegex)
	{
		TargetFullName = targetFullName;
		ClassRegex = classRegex;

		CachedHashCode = new Lazy<int>(() => HashCode.Combine(TargetFullName, ClassRegex));
	}

	public bool Equals(DeclaredRoslynjectModule other) =>
		string.Equals(TargetFullName, other.TargetFullName, StringComparison.OrdinalIgnoreCase)
		&& string.Equals(ClassRegex!, other.ClassRegex!, StringComparison.OrdinalIgnoreCase);

	public override bool Equals(object obj) =>
		obj is DeclaredRoslynjectModule other
		&& other.Equals(this);

	public override int GetHashCode() => CachedHashCode.Value;
}
