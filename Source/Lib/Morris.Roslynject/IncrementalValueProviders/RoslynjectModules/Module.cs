using Microsoft.CodeAnalysis;
using Morris.Roslynject.Extensions;
using Morris.Roslynject.IncrementalValueProviders.DeclaredRoslynjectModuleAttributes;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Morris.Roslynject.IncrementalValueProviders.RoslynjectModules;

internal sealed class Module
{
	public readonly DeclaredRoslynjectModule DeclaredModule;
	public readonly ImmutableArray<AttributeAndRegistrations> AttributeAndRegistrations;

	private readonly Lazy<int> CachedHashCode;

	public Module(
		DeclaredRoslynjectModule declaredModule,
		IEnumerable<INamedTypeSymbol> injectionCandidates)
	{
		DeclaredModule = declaredModule;
		if (declaredModule.ClassRegex is not null)
			injectionCandidates = injectionCandidates
				.Where(x => Regex.IsMatch(x.ToDisplayString(), declaredModule.ClassRegex));

		AttributeAndRegistrations = CreateAttributeRegistrations(declaredModule, injectionCandidates);

		CachedHashCode = new Lazy<int>(() =>
			HashCode.Combine(
				DeclaredModule,
				AttributeAndRegistrations.GetContentsHashCode()
			)
		);
	}

	private ImmutableArray<AttributeAndRegistrations> CreateAttributeRegistrations(
		DeclaredRoslynjectModule declaredModule,
		IEnumerable<INamedTypeSymbol> injectionCandidates)
	{
		var builder = ImmutableArray.CreateBuilder<AttributeAndRegistrations>();
		foreach(DeclaredRoslynjectAttribute declaredAttribute in declaredModule.RoslynjectAttributes)
		{
			var item = new AttributeAndRegistrations(
				declaredRoslynjectAttribute: declaredAttribute,
				injectionCandidates: injectionCandidates);
			builder.Add(item);
		}
		return builder.ToImmutable();
	}
}
