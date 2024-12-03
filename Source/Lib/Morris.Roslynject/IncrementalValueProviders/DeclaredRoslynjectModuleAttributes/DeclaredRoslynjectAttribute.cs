using Microsoft.CodeAnalysis;

namespace Morris.Roslynject.IncrementalValueProviders.DeclaredRoslynjectModuleAttributes;

internal class DeclaredRoslynjectAttribute : IEquatable<DeclaredRoslynjectAttribute>
{
	public readonly Find Find;
	public readonly INamedTypeSymbol Type;
	public readonly Register Register;
	public readonly WithLifetime WithLifetime;
	public readonly string? ClassRegex;
	public readonly string? ServiceKeyRegex;

	private readonly Lazy<int> CachedHashCode;

	public DeclaredRoslynjectAttribute(
		Find find,
		INamedTypeSymbol type,
		Register register,
		WithLifetime withLifetime,
		string? classRegex,
		string? serviceKeyRegex)
	{
		Find = find;
		Type = type;
		Register = register;
		WithLifetime = withLifetime;
		ClassRegex = classRegex;
		ServiceKeyRegex = serviceKeyRegex;

		CachedHashCode = new Lazy<int>(() =>
			HashCode.Combine(
				Find,
				Register,
				WithLifetime,
				ClassRegex,
				ServiceKeyRegex,
				SymbolEqualityComparer.Default.GetHashCode(Type)
			)
		);
	}

	public bool Equals(DeclaredRoslynjectAttribute other) =>
		Find == other.Find
		&& Register == other.Register
		&& WithLifetime == other.WithLifetime
		&& string.Equals(ClassRegex, other.ClassRegex, StringComparison.OrdinalIgnoreCase)
		&& string.Equals(ServiceKeyRegex, other.ServiceKeyRegex, StringComparison.OrdinalIgnoreCase)
		&& SymbolEqualityComparer.Default.Equals(Type, other.Type);

	public override bool Equals(object obj) =>
		obj is DeclaredRoslynjectAttribute other
		&& other.Equals(this);

	public override int GetHashCode() => CachedHashCode.Value;
}
