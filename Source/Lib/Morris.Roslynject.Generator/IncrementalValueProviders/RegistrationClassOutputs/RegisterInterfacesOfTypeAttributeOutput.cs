using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Morris.Roslynject.Generator.IncrementalValueProviders.RegistrationClassOutputs;

internal class RegisterInterfacesOfTypeAttributeOutput : RegisterAttributeOutputBase
{
    public RegisterInterfacesOfTypeAttributeOutput(
        string attributeSourceCode,
        ServiceLifetime serviceLifetime)
        : base(
            attributeSourceCode: attributeSourceCode,
            serviceLifetime: serviceLifetime)
    {
    }

    public override void GenerateCode(Action<string> writeLine)
    {
        throw new NotImplementedException();
    }

}
