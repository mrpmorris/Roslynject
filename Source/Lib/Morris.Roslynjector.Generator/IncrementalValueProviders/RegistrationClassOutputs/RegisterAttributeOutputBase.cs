using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Morris.Roslynjector.Generator.IncrementalValueProviders.RegistrationClassOutputs;

internal abstract class RegisterAttributeOutputBase
{
    public readonly AttributeSyntax AttributeSyntax;
    public readonly ServiceLifetime ServiceLifetime;

    public abstract void GenerateCode(Action<string> writeLine);

    protected RegisterAttributeOutputBase(
        AttributeSyntax attributeSyntax,
        ServiceLifetime serviceLifetime)
    {
        AttributeSyntax = attributeSyntax;
        ServiceLifetime = serviceLifetime;
    }
}
