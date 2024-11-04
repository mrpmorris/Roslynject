using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Morris.Roslynjector.Generator.IncrementalValueProviders.RegistrationClassOutputs;

internal class RegisterInterfacesWhereDescendsFromAttributeOutput : RegisterAttributeOutputBase
{
    public RegisterInterfacesWhereDescendsFromAttributeOutput(
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
