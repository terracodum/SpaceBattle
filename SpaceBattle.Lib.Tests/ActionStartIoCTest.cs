using System;
using System.Collections.Generic;
using Hwdtech;
using Hwdtech.Ioc;
using Moq;
using StarWars.Lib;
using Xunit;

namespace StarWars.Test;

public class MoveCommandIoCTests
{
    public MoveCommandIoCTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        var iocScope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", iocScope).Execute();
    }

    [Fact]
    public void Execute_ShouldRegisterMoveCommandDependency()
    {
        var mockInjectable = new Mock<ICommandInjectable>();
        var mockQueue = new Mock<ICommandReceiver>();
        var mockCmd = new Mock<Hwdtech.ICommand>();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.Move",
            (Func<object, object>)(obj => mockCmd.Object)).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.CommandInjectable",
            (Func<object, object>)(obj => mockInjectable.Object)).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.Send",
            (Func<object, object>)(obj => mockCmd.Object)).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.Macro",
            (Func<object, object>)(obj => mockCmd.Object)).Execute();

        new RegisterIoCDependencyActionsStart().Execute();

        var order = new Dictionary<string, object>
        {
            ["gameObject"] = new Dictionary<string, object>(),
            ["queue"] = mockQueue.Object,
            ["cmdType"] = "Move"
        };

        var cmd = IoC.Resolve<Hwdtech.ICommand>("Actions.Start", order);
        Assert.NotNull(cmd);
        cmd.Execute();
    }
}
