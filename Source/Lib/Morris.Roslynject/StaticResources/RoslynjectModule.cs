namespace Morris.Roslynject.StaticResources;

internal static partial class SourceCode
{
	public const string RoslynjectModule =
	"""
		[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
		internal class RoslynjectModuleAttribute : Attribute
		{
		}
	""";
}
