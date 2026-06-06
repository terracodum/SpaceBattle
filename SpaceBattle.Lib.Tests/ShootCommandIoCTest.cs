using System;
using System.Collections.Generic;
using Hwdtech;
using Hwdtech.Ioc;
using Moq;
using StarWars.Lib;
using Xunit;

namespace StarWars.Test;

public class ShootCommandIoCTest
{
    public ShootCommandIoCTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        var scope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();
    }

    [Fact]
    public void Execute_ShouldRegisterShootCommandDependency()
    {
        var mockShootable = new Mock<IShootable>();
        var mockRepo = new Mock<IGameObjectRepository>();
        var mockQueue = new Mock<ICommandReceiver>();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Adapters.IShootable",
            (Func<object, object>)(obj => mockShootable.Object)).Execute();

        new RegisterIoCDependencyShootCommand().Execute();

        var order = new Dictionary<string, object>
        {
            ["gameObject"] = new Dictionary<string, object>(),
            ["repository"] = mockRepo.Object,
            ["queue"] = mockQueue.Object
        };

        var cmd = IoC.Resolve<StarWars.Lib.ICommand>("Commands.Shoot", order);

        Assert.NotNull(cmd);
        Assert.IsType<ShootCommand>(cmd);
    }
}
