using Microsoft.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Morris.Roslynject.Extensions;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Morris.Roslynject.IncrementalValueProviders.RegistrationClassOutputs;

internal class RegisterClassesDescendedFromOutput :
	RegisterAttributeOutputBase,
	IEquatable<RegisterClassesDescendedFromOutput>
{
	public readonly INamedTypeSymbol BaseClassType;
	public readonly RegisterClassAs RegisterClassAs;
	public readonly string? ClassRegex;
	public readonly ImmutableArray<INamedTypeSymbol> ClassesToRegister;
	private readonly Lazy<int> CachedHashCode;

	public static RegisterAttributeOutputBase? Create(
		string attributeSourceCode,
		INamedTypeSymbol baseClassType,
		ServiceLifetime serviceLifetime,
		RegisterClassAs registerClassAs,
		string? classRegex,
		ImmutableArray<INamedTypeSymbol> injectionCandidates)
	{
		Regex? regex =
			classRegex is null
			? null
			: new Regex(classRegex!);
		Func<INamedTypeSymbol, bool> regexMatch =
			regex is null
			? _ => true
			: x => regex.IsMatch(x.ToDisplayString());

		var classesToRegister =
			injectionCandidates
			.Where(x => x.DescendsFrom(baseClassType))
			.Where(regexMatch)
			.ToImmutableArray();

		return
			classesToRegister.Length == 0
			? null
			: new RegisterClassesDescendedFromOutput(
				attributeSourceCode: attributeSourceCode,
				baseClassType: baseClassType,
				serviceLifetime: serviceLifetime,
				registerClassAs: registerClassAs,
				classRegex: classRegex,
				classesToRegister: classesToRegister);
	}


	public override bool Equals(object? obj) =>
		obj is RegisterClassesDescendedFromOutput other
		&& Equals(other);

	public bool Equals(RegisterClassesDescendedFromOutput? other) =>
		base.Equals(other)
		&& TypeIdentityComparer.Default.Equals(
			BaseClassType,
			other.BaseClassType
		)
		&& Enumerable.SequenceEqual(
			ClassesToRegister,
			other.ClassesToRegister,
			SymbolEqualityComparer.Default
		);

	public override void GenerateCode(Action<string> writeLine)
	{
		string? GetBaseClassRegistration(INamedTypeSymbol symbol)
		{
			INamedTypeSymbol? resultSymbol = RegisterClassAs switch {
				RegisterClassAs.DescendantClass => null,
				RegisterClassAs.BaseClass => BaseClassType,
				RegisterClassAs.BaseClosedGenericClass => symbol.GetBaseClosedGenericType(BaseClassType),
				_ => throw new NotImplementedException(RegisterClassAs.ToString())
			};
			return resultSymbol is null
				? null
				: resultSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
		}

		foreach (INamedTypeSymbol classToRegister in ClassesToRegister)
		{
			string implementingClassFullName = classToRegister.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
			string? serviceKey = GetBaseClassRegistration(classToRegister);
			if (serviceKey is null)
				writeLine($"services.Add{ServiceLifetime}(typeof({implementingClassFullName}));");
			else
				writeLine($"services.Add{ServiceLifetime}(typeof({serviceKey}), typeof({implementingClassFullName}));");
		}
	}

	public override int GetHashCode() => CachedHashCode.Value;

	private RegisterClassesDescendedFromOutput(
		string attributeSourceCode,
		INamedTypeSymbol baseClassType,
		ServiceLifetime serviceLifetime,
		RegisterClassAs registerClassAs,
		string? classRegex,
		ImmutableArray<INamedTypeSymbol> classesToRegister)
		: base(
			attributeSourceCode: attributeSourceCode,
			serviceLifetime: serviceLifetime)
	{
		BaseClassType = baseClassType;
		ClassesToRegister = classesToRegister;
		RegisterClassAs = registerClassAs;
		ClassRegex = classRegex;
		CachedHashCode = new Lazy<int>(() =>
			HashCode
			.Combine(
				base.GetHashCode(),
				TypeIdentityComparer.Default.GetHashCode(BaseClassType),
				RegisterClassAs,
				ClassRegex,
				ClassesToRegister.GetContentsHashCode()
			)
		);
	}

}
