using System;
using System.Collections.Generic;
using Hwdtech;
using Hwdtech.Ioc;
using Moq;
using StarWars.Lib;
using Xunit;

namespace StarWars.Test;

public class RotateCommandIoCTests
{
    public RotateCommandIoCTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        var iocScope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", iocScope).Execute();
    }

    [Fact]
    public void Execute_ShouldRegisterMoveCommandDependency()
    {
        var mockRotating = new Mock<IRotatingObject>();
        var mockGameObject = new Mock<IDictionary<string, object>>();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Adapters.IRotatingObject",
            (Func<object, IRotatingObject>)(obj =>
            {
                return mockRotating.Object;
            })).Execute();

        new RegisterIoCDependencyRotateCommand().Execute();

        var rotateCommand = IoC.Resolve<StarWars.Lib.ICommand>("Commands.Rotate", mockGameObject.Object);
        Assert.NotNull(rotateCommand);
        Assert.IsType<RotateCommand>(rotateCommand);
    }
}
