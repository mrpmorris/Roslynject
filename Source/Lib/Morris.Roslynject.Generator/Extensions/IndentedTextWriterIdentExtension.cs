using System.CodeDom.Compiler;
using System.Runtime.CompilerServices;

namespace Morris.Roslynject.Generator.Extensions;

internal static partial class IndentedTextWriterExtensions
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IDisposable IndentedBlock(this IndentedTextWriter writer)
	{
		writer.Indent++;
		return new DisposableAction(() => writer.Indent--);
	}
}
