using System;
using System.Collections.Generic;
using Hwdtech;
using Hwdtech.Ioc;
using Moq;
using StarWars.Lib;
using Xunit;

namespace StarWars.Test;

public class RegisterIoCDependencyMacroMoveRotateTests
{
    public RegisterIoCDependencyMacroMoveRotateTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        var iocScope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", iocScope).Execute();
    }

    [Fact]
    public void Execute_MacroMove_ResolvesAndExecutesAllCommands()
    {
        var mockMoveCmd = new Mock<Hwdtech.ICommand>();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Specs.Move",
            (Func<object[], object>)(a => new List<string> { "Commands.MoveStep" })).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.MoveStep",
            (Func<object[], object>)(a => mockMoveCmd.Object)).Execute();

        new RegisterIoCDependencyMacroMoveRotate().Execute();

        var macroMove = IoC.Resolve<Hwdtech.ICommand>("Macro.Move", Array.Empty<object>());
        macroMove.Execute();

        mockMoveCmd.Verify(c => c.Execute(), Times.Once);
    }

    [Fact]
    public void Execute_MacroRotate_ResolvesAndExecutesAllCommands()
    {
        var mockRotateCmd = new Mock<Hwdtech.ICommand>();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Specs.Rotate",
            (Func<object[], object>)(a => new List<string> { "Commands.RotateStep" })).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.RotateStep",
            (Func<object[], object>)(a => mockRotateCmd.Object)).Execute();

        new RegisterIoCDependencyMacroMoveRotate().Execute();

        var macroRotate = IoC.Resolve<Hwdtech.ICommand>("Macro.Rotate", Array.Empty<object>());
        macroRotate.Execute();

        mockRotateCmd.Verify(c => c.Execute(), Times.Once);
    }
}
