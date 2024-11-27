namespace Morris.Roslynject.Generator.StaticResources;

[AttributeUsage(AttributeTargets.Class)]
internal class BumAttribute : Attribute
{
	public string Name { get; set; }

	public BumAttribute(string name)
	{
		Name = name;
	}
}
