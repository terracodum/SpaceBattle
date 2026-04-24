using System.Collections.Generic;
using Hwdtech;
using Hwdtech.Ioc;
using Moq;
using StarWars.Lib;
using Xunit;

namespace StarWars.Test;

public class ActionsStopIoCTests
{
    public ActionsStopIoCTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        var iocScope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", iocScope).Execute();
    }

    [Fact]
    public void Execute_ShouldRegisterStopCommandDependency()
    {
        var mockInjectable = new Mock<ICommandInjectable>();

        var gameObject = new Dictionary<string, object>
        {
            ["repeatableMove"] = mockInjectable.Object
        };
        Hwdtech.ICommand injected = null;
        mockInjectable.Setup(x => x.Inject(It.IsAny<Hwdtech.ICommand>())).Callback<Hwdtech.ICommand>(c => injected = c);

        new RegisterIoCDependencyActionsStop().Execute();

        var order = new Dictionary<string, object>
        {
            ["gameObject"] = gameObject,
            ["cmdType"] = "Move"
        };

        var cmd = IoC.Resolve<Hwdtech.ICommand>("Actions.Stop", order);

        cmd.Execute();
        Assert.NotNull(cmd);

        Assert.NotNull(injected);
        injected.Execute();

        mockInjectable.Verify(x => x.Inject(It.IsAny<Hwdtech.ICommand>()), Times.Once);
    }
}
