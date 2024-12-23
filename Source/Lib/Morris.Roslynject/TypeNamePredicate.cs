using Microsoft.CodeAnalysis;
using System;
using System.Text.RegularExpressions;

namespace Morris.Roslynject;

internal static class TypeNamePredicate
{
	private static readonly Lazy<Func<INamedTypeSymbol, bool>> MatchAll = new(() => _ => true);

	public static Lazy<Func<INamedTypeSymbol, bool>> Create(string? regex) =>
		regex is null
		? MatchAll
		: new(() =>
		{
			var r = new Regex(regex);
			return x => r.IsMatch(x.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
		});
}
