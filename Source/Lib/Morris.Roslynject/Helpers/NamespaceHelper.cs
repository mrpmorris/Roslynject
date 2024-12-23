﻿using Microsoft.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Morris.Roslynject.Helpers;

internal static class NamespaceHelper
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static string Combine(
		string? namespaceName,
		string className)
	=>
		namespaceName is null
		? className
		: $"{namespaceName}.{className}";

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static string Combine(
		INamespaceSymbol namespaceSymbol,
		string className)
	=>
		namespaceSymbol.IsGlobalNamespace
		? className
		: $"{namespaceSymbol.ToDisplayString()}.{className}";
}