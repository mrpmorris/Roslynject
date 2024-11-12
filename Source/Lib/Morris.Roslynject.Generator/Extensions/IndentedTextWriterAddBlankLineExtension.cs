using System.CodeDom.Compiler;

namespace Morris.Roslynject.Generator.Extensions;
internal static class IndentedTextWriterAddBlankLineExtension
{
	public static void AddBlankLine(this IndentedTextWriter writer)
	{
		int originalIndent = writer.Indent;
		writer.Indent = 0;
		writer.WriteLine();
		writer.Indent = originalIndent;
	}
}
