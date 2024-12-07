namespace Morris.Roslynject.IncrementalValueProviders.RoslynjectModules;

internal sealed class RoslynjectRegistration
{
	public readonly string? ServiceKeyTypeName;
	public readonly string ServiceTypeName;
	public readonly WithLifetime WithLifetime;

	private readonly Lazy<int> CachedHashCode;

	public RoslynjectRegistration(
		WithLifetime withLifetime,
		string? serviceKeyTypeName,
		string serviceTypeName)
	{
		WithLifetime = withLifetime;
		ServiceKeyTypeName = serviceKeyTypeName;
		ServiceTypeName = serviceTypeName;

		CachedHashCode = new Lazy<int>(() =>
			HashCode.Combine(
				WithLifetime,
				ServiceKeyTypeName,
				ServiceTypeName
			)
		);
	}

	public override bool Equals(object obj) =>
		obj is RoslynjectRegistration other
		&& other.WithLifetime == WithLifetime
		&& other.ServiceKeyTypeName == ServiceKeyTypeName
		&& other.ServiceTypeName == ServiceTypeName;

	public override int GetHashCode() => CachedHashCode.Value;
}
