using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;

namespace Morris.Roslynject.Extensions;

internal static class ImmutableDictionaryStringObjectExtensions
{
	public static TValue GetValue<TValue>(
		this ImmutableDictionary<string, object?> dictionary,
		string key)
	=>
		(TValue)dictionary[key!]!;

	public static TValue GetValueOrDefault<TValue>(
		this ImmutableDictionary<string, object?> dictionary,
		string key,
		TValue? defaultValue = default!)
	=>
		dictionary.TryGetValue(key, out TValue? value)
		? value!
		: defaultValue!;

	public static bool TryGetValue<TValue>(
		this ImmutableDictionary<string, object?> dictionary,
		string key,
		out TValue? value)
	{
		if (!dictionary.TryGetValue(key, out object? valueObj))
		{
			value = default;
			return false;
		}
		value = (TValue?)valueObj;
		return true;
	}
}
