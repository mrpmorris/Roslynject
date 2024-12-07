using Microsoft.CodeAnalysis;
using Morris.Roslynject.Extensions;
using Morris.Roslynject.IncrementalValueProviders.DeclaredRoslynjectModuleAttributes;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Morris.Roslynject.IncrementalValueProviders.RoslynjectModules;

internal sealed class AttributeAndRegistrations
{
	public readonly DeclaredRoslynjectAttribute DeclaredRoslynjectAttribute;
	public readonly ImmutableArray<ServiceRegistration> Registrations;

	private readonly Lazy<int> CachedHashCode;

	public AttributeAndRegistrations(
		DeclaredRoslynjectAttribute declaredRoslynjectAttribute,
		IEnumerable<INamedTypeSymbol> injectionCandidates)
	{
		DeclaredRoslynjectAttribute = declaredRoslynjectAttribute;
		Registrations = CreateRegistrations(declaredRoslynjectAttribute, injectionCandidates);

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

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private ImmutableArray<ServiceRegistration> CreateRegistrations(
		DeclaredRoslynjectAttribute declaredRoslynjectAttribute,
		IEnumerable<INamedTypeSymbol> injectionCandidates)
	{
		injectionCandidates = FilterInjectionCandidates(declaredRoslynjectAttribute, injectionCandidates);

		var builder = ImmutableArray.CreateBuilder<ServiceRegistration>();
		foreach (var item in injectionCandidates)
		{
			var serviceRegistration = new ServiceRegistration(
				withLifetime: declaredRoslynjectAttribute.WithLifetime,
				serviceKeyTypeName: item.ToDisplayString(),
				serviceTypeName: item.ToDisplayString()
			);
			builder.Add(serviceRegistration);
		}
		return builder.ToImmutable();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IEnumerable<INamedTypeSymbol> FilterInjectionCandidates(
		DeclaredRoslynjectAttribute declaredRoslynjectAttribute,
		IEnumerable<INamedTypeSymbol> injectionCandidates)
	{
		Func<INamedTypeSymbol, bool> classMatches =
			declaredRoslynjectAttribute.ClassRegex is null
			? _ => true
			: x => Regex.IsMatch(x.ToDisplayString(), declaredRoslynjectAttribute.ClassRegex);
		Func<INamedTypeSymbol, bool> keyMatches =
			declaredRoslynjectAttribute.ServiceKeyRegex is null
			? _ => true
			: x => Regex.IsMatch(x.ToDisplayString(), declaredRoslynjectAttribute.ServiceKeyRegex);
		Func<INamedTypeSymbol, bool> isMatch = x => classMatches(x) && keyMatches(x);
		injectionCandidates = injectionCandidates.Where(isMatch);
		return injectionCandidates;
	}
}
