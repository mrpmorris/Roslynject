using Morris.Roslynject.Extensions;
using Morris.Roslynject.IncrementalValueProviders.DeclaredRoslynjectModuleAttributes;
using System.Collections.Immutable;

namespace Morris.Roslynject.IncrementalValueProviders.RoslynjectModules;

internal sealed class AttributeAndRegistrations
{
	public readonly DeclaredRoslynjectAttribute DeclaredRoslynjectAttribute;
	public readonly ImmutableArray<ServiceRegistration> Registrations;

	private readonly Lazy<int> CachedHashCode;

	public AttributeAndRegistrations(
		DeclaredRoslynjectAttribute declaredRoslynjectAttribute,
		ImmutableArray<ServiceRegistration> registrations)
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
		obj is AttributeAndRegistrations other
		&& other.DeclaredRoslynjectAttribute.Equals(DeclaredRoslynjectAttribute)
		&& other.Registrations.SequenceEqual(Registrations);

	public override int GetHashCode() => CachedHashCode.Value;
}
