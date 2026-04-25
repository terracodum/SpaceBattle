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
        var mockMoving = new Mock<IMovingObject>();
        var mockGameObject = new Mock<IDictionary<string, object>>();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Adapters.IMovingObject",
            (Func<object, IMovingObject>)(obj =>
            {
                return mockMoving.Object;
            })).Execute();

        new RegisterIoCDependencyMoveCommand().Execute();

        var moveCommand = IoC.Resolve<StarWars.Lib.ICommand>("Commands.Move", mockGameObject.Object);
        Assert.NotNull(moveCommand);
        Assert.IsType<MoveCommand>(moveCommand);
    }
}
