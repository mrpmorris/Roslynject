using System.CodeDom.Compiler;

namespace Morris.Roslynject.Extensions;

internal static partial class IndentedTextWriterExtensions
{
	public static void AddBlankLine(this IndentedTextWriter writer)
	{
		int originalIndent = writer.Indent;
		writer.Indent = 0;
		writer.WriteLine();
		writer.Indent = originalIndent;
	}
}
