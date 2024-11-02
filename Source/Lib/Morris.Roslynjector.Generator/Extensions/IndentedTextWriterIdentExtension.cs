using System.CodeDom.Compiler;
using System.Runtime.CompilerServices;

namespace Morris.Roslynjector.Generator.Extensions;

internal static class IndentedTextWriterIndentExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IDisposable IndentedBlock(this IndentedTextWriter writer)
    {
        writer.Indent++;
        return new DisposableAction(() => writer.Indent--);
    }
}
