using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morris.RoslynjectTests;

[TestClass]
public class FindClassDescendantsAndRegisterBaseType
{
	[TestMethod]
	public void Test()
	{
		string sourceCode =
			$$"""
			using Morris.Roslynject;
			namespace Tests
			{
				public class BaseClass { }
				public class Child1 : BaseClass { }
				public class Child2 : BaseClass { }

				[Roslynject(Find.DescendantsOf, typeof(BaseClass), Register.BaseType, WithLifetime.Scoped)]
				public partial class MyModule {}
			}
			""";

		string expectedGeneratedCode = "";

		SourceGeneratorExecutor
			.AssertGeneratedCodeMatches(
				sourceCode: sourceCode,
				expectedGeneratedCode: expectedGeneratedCode
			);
	}
}
