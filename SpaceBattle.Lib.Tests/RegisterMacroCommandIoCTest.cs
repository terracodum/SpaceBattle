using System.Collections.Generic;
using Hwdtech;
using Hwdtech.Ioc;
using Moq;
using StarWars.Lib;
using Xunit;

namespace StarWars.Test;

public class RegisterMacroCommandIoCTests
{
    public RegisterMacroCommandIoCTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        var iocScope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", iocScope).Execute();
    }

    [Fact]
    public void Execute_ShouldRegisterMacroCommandDependency()
    {
        var mockCmd1 = new Mock<Hwdtech.ICommand>();
        var mockCmd2 = new Mock<Hwdtech.ICommand>();

        new RegisterIoCDependencyMacroCommand().Execute();

        var commands = new List<Hwdtech.ICommand> { mockCmd1.Object, mockCmd2.Object };
        var macroCommand = IoC.Resolve<Hwdtech.ICommand>("Commands.Macro", commands);

        Assert.NotNull(macroCommand);
        Assert.IsType<MacroCommand>(macroCommand);
    }
}
