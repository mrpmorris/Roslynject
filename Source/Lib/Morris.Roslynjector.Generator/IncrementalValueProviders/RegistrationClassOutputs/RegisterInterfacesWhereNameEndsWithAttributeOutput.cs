using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Morris.Roslynjector.Generator.IncrementalValueProviders.RegistrationClassOutputs;

internal class RegisterInterfacesWhereNameEndsWithAttributeOutput : RegisterAttributeOutputBase
{
    public RegisterInterfacesWhereNameEndsWithAttributeOutput(
        AttributeSyntax attributeSyntax,
        ServiceLifetime serviceLifetime)
        : base(
            attributeSyntax: attributeSyntax,
            serviceLifetime: serviceLifetime)
    {
    }

    public override void GenerateCode(Action<string> writeLine)
    {
        throw new NotImplementedException();
    }

}
