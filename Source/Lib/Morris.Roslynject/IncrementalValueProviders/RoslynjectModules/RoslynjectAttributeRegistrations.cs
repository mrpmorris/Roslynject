using Morris.Roslynject.Extensions;
using Morris.Roslynject.IncrementalValueProviders.DeclaredRoslynjectModuleAttributes;
using System.Collections.Immutable;

namespace Morris.Roslynject.IncrementalValueProviders.RoslynjectModules;

internal sealed class RoslynjectAttributeRegistrations
{
	public readonly DeclaredRoslynjectAttribute DeclaredRoslynjectAttribute;
	public readonly ImmutableArray<RoslynjectRegistration> Registrations;

	private readonly Lazy<int> CachedHashCode;

	public RoslynjectAttributeRegistrations(
		DeclaredRoslynjectAttribute declaredRoslynjectAttribute,
		ImmutableArray<RoslynjectRegistration> registrations)
	{
		DeclaredRoslynjectAttribute = declaredRoslynjectAttribute;
		Registrations = registrations;

		CachedHashCode = new Lazy<int>(() =>
			HashCode.Combine(
				DeclaredRoslynjectAttribute,
				Registrations.GetContentsHashCode()
			)
		);
	}

	public override bool Equals(object obj) =>
		obj is RoslynjectAttributeRegistrations other
		&& other.DeclaredRoslynjectAttribute.Equals(DeclaredRoslynjectAttribute)
		&& other.Registrations.SequenceEqual(Registrations);

	public override int GetHashCode() => CachedHashCode.Value;
}
