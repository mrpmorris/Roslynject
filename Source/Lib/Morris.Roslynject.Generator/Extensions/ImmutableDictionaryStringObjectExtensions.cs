using System.Collections.Immutable;

namespace Morris.Roslynject.Generator.Extensions;

internal static class ImmutableDictionaryStringObjectExtensions
{
	public static TValue GetValue<TValue>(
		this ImmutableDictionary<string, object?> dictionary,
		string key)
	=>
		(TValue)dictionary[key!]!;

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
