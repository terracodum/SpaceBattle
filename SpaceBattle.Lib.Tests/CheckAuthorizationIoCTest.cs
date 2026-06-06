using System;
using System.Collections.Generic;
using Hwdtech;
using Hwdtech.Ioc;
using Moq;
using StarWars.Lib;
using Xunit;

namespace StarWars.Test;

public class CheckAuthorizationIoCTest
{
    public CheckAuthorizationIoCTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        var scope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();
    }

    [Fact]
    public void Execute_ShouldRegisterCheckAuthorizationDependency()
    {
        var mockAuth = new Mock<IAuthorizationService>();
        mockAuth.Setup(x => x.IsAuthorized("game1", "ship1", "player1")).Returns(true);

        new RegisterIoCDependencyCheckAuthorization().Execute();

        var order = new Dictionary<string, object>
        {
            ["authService"] = mockAuth.Object,
            ["gameId"] = "game1",
            ["objectId"] = "ship1",
            ["playerId"] = "player1"
        };

        var cmd = IoC.Resolve<Hwdtech.ICommand>("Commands.CheckAuthorization", order);

        Assert.NotNull(cmd);
        Assert.IsType<CheckAuthorizationCommand>(cmd);

        cmd.Execute();

        mockAuth.Verify(x => x.IsAuthorized("game1", "ship1", "player1"), Times.Once);
    }

    [Fact]
    public void Execute_UnauthorizedPlayer_ThrowsFromResolvedCommand()
    {
        var mockAuth = new Mock<IAuthorizationService>();
        mockAuth.Setup(x => x.IsAuthorized(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(false);

        new RegisterIoCDependencyCheckAuthorization().Execute();

        var order = new Dictionary<string, object>
        {
            ["authService"] = mockAuth.Object,
            ["gameId"] = "game1",
            ["objectId"] = "ship1",
            ["playerId"] = "hacker"
        };

        var cmd = IoC.Resolve<Hwdtech.ICommand>("Commands.CheckAuthorization", order);

        Assert.Throws<UnauthorizedAccessException>(() => cmd.Execute());
    }
}
