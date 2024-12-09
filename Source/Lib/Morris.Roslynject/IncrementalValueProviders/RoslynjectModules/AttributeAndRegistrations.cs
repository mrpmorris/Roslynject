using Microsoft.CodeAnalysis;
using Morris.Roslynject.Extensions;
using Morris.Roslynject.IncrementalValueProviders.DeclaredRoslynjectModuleAttributes;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Morris.Roslynject.Extensions;

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
		Func<INamedTypeSymbol, bool> classFullNameMatches =
			declaredRoslynjectAttribute.ClassRegex is null
			? _ => true
			: x => Regex.IsMatch(x.ToDisplayString(), declaredRoslynjectAttribute.ClassRegex);

		Func<INamedTypeSymbol, bool> keyFullNameMatches =
			declaredRoslynjectAttribute.ServiceKeyRegex is null
			? _ => true
			: x => Regex.IsMatch(x.ToDisplayString(), declaredRoslynjectAttribute.ServiceKeyRegex);

		Func<INamedTypeSymbol, bool> inheritanceMatches = declaredRoslynjectAttribute.Find switch {
			Find.AnyTypeOf => x => SymbolEqualityComparer.Default.Equals(x, declaredRoslynjectAttribute.Type),
			Find.DescendantsOf => x => x.DescendsFrom(declaredRoslynjectAttribute.Type),
			Find.Exactly => x => x.IsTypeOf(declaredRoslynjectAttribute.Type),
			_ => throw new NotImplementedException(declaredRoslynjectAttribute.Find.ToString())
		};

		Func<INamedTypeSymbol, bool> typeIsMatch = x =>
			classFullNameMatches(x)
			&& keyFullNameMatches(x)
			&& inheritanceMatches(x);

		return declaredRoslynjectAttribute.Type.TypeKind switch {
			TypeKind.Class => injectionCandidates.Where(typeIsMatch),
			TypeKind.Interface => injectionCandidates.Where(x => x.Interfaces.Any(typeIsMatch)),
			_ => throw new NotImplementedException(declaredRoslynjectAttribute.Type.TypeKind.ToString())
		};
	}
}
